using System;
using System.Buffers;
using System.Text;

namespace Airmiss.Protocol.Tcp
{
    internal class JsonBuffer : IDisposable
    {
        private readonly int _bufferSize;
        private readonly int _maxBufferSize;
        private readonly ArrayPool<byte> _arrayPool;

        private byte[] _buffer;
        private int _jsonStartPos;
        private int _jsonCurPos;
        private int _jsonStackedBrackets;
        private bool _jsonStarted;
        private bool _insideQuotes;
        private bool _isEscaping;

        public JsonBuffer(int bufferSize, int maxBufferSize = 0, ArrayPool<byte> arrayPool = null)
        {
            _bufferSize = bufferSize;
            _maxBufferSize = maxBufferSize;
            _arrayPool = arrayPool ?? ArrayPool<byte>.Shared;
            _buffer = _arrayPool.Rent(bufferSize);
        }

        public byte[] Buffer => _buffer;

        public int BufferCurPos {get;set;}

        public bool TryExtractJsonFromBuffer(out byte[] json)
        {
            if (BufferCurPos > _buffer.Length)
            {
                throw new ArgumentException("Buffer current pos or length value is invalid");
            }

            json = null;
            int jsonLenght = 0;

            for (int i = _jsonCurPos; i < BufferCurPos; i++)
            {
                _jsonCurPos = i + 1;

                if (_buffer[i] == '"' && !_isEscaping)
                {
                    _insideQuotes = !_insideQuotes;
                }

                if (!_insideQuotes)
                {
                    if (_buffer[i] == '{')
                    {
                        _jsonStackedBrackets++;
                        if (!_jsonStarted)
                        {
                            _jsonStartPos = i;
                            _jsonStarted = true;
                        }
                    }
                    else if (_buffer[i] == '}')
                    {
                        _jsonStackedBrackets--;
                    }

                    if (_jsonStarted &&
                        _jsonStackedBrackets == 0)
                    {
                        jsonLenght = i - _jsonStartPos + 1;
                        break;
                    }
                }
                else
                {
                    if (_isEscaping)
                    {
                        _isEscaping = false;
                    }
                    else if (_buffer[i] == '\\')
                    {
                        _isEscaping = true;
                    }
                }
            }

            if (jsonLenght > 1)
            {
                json = new byte[jsonLenght];
                System.Buffer.BlockCopy(_buffer, _jsonStartPos, json, 0, jsonLenght);

                // Shifts the buffer to the left
                BufferCurPos -= (jsonLenght + _jsonStartPos);
                System.Buffer.BlockCopy(_buffer, jsonLenght + _jsonStartPos, _buffer, 0, BufferCurPos);

                Reset();

                return true;
            }

            return false;
        }

        public void IncreaseBuffer()
        {
            if (_maxBufferSize == 0 || _buffer.Length + _bufferSize > _maxBufferSize)
            {
                throw new OverflowException($"Maximum buffer size reached. The current buffer size is {_buffer.Length} and the buffer segment size is {_bufferSize}.");
            }

            var currentBuffer = _buffer;
            var increasedBuffer = _arrayPool.Rent(_buffer.Length + _bufferSize);
            System.Buffer.BlockCopy(currentBuffer, 0, increasedBuffer, 0, currentBuffer.Length);
            _buffer = increasedBuffer;
            _arrayPool.Return(currentBuffer, true);
        }

        public static bool TryExtractJsonFromBuffer(ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> json)
        {
            var jsonLength = 0;
            var jsonStackedBrackets = 0;
            var jsonStartPos = 0;
            var jsonStarted = false;
            var insideQuotes = false;
            var isEscaping = false;
            var i = 0;

            foreach (var segment in buffer)
            {
                foreach (var c in segment.Span)
                {
                    if (c == '"' && !isEscaping)
                    {
                        insideQuotes = !insideQuotes;
                    }

                    if (!insideQuotes)
                    {
                        if (c == '{')
                        {
                            jsonStackedBrackets++;
                            if (!jsonStarted)
                            {
                                jsonStartPos = i;
                                jsonStarted = true;
                            }
                        }
                        else if (c == '}')
                        {
                            jsonStackedBrackets--;
                        }

                        if (jsonStarted &&
                            jsonStackedBrackets == 0)
                        {
                            jsonLength = i - jsonStartPos + 1;
                            break;
                        }
                    }
                    else
                    {
                        if (isEscaping)
                        {
                            isEscaping = false;
                        }
                        else if (c == '\\')
                        {
                            isEscaping = true;
                        }
                    }

                    if (jsonLength > 1) break;
                    i++;
                }
            }

            if (jsonLength > 1)
            {
                json = buffer.Slice(jsonStartPos, jsonLength);
                return true;
            }

            json = default;
            return false;
        }

        private void Reset()
        {
            _jsonCurPos = 0;
            _jsonStartPos = 0;
            _jsonStarted = false;
            _insideQuotes = false;
            _isEscaping = false;

            if (_buffer.Length > _bufferSize && BufferCurPos < _bufferSize)
            {
                var currentBuffer = _buffer;
                var decreasedBuffer = _arrayPool.Rent(_bufferSize);
                System.Buffer.BlockCopy(currentBuffer, 0, decreasedBuffer, 0, _bufferSize);
                _buffer = decreasedBuffer;
                _arrayPool.Return(currentBuffer);
            }
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(_buffer, 0, _buffer.Length);
        }

        public void Dispose()
        {
            _arrayPool.Return(_buffer);
        }
    }
}
