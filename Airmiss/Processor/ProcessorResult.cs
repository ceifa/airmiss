using System;

namespace Airmiss.Processor
{
    public struct ProcessorResult
    {
        public ProcessorResult(Type type, object? result)
        {
            Type = type;
            Result = result;
        }

        public Type Type { get; }

        public object? Result { get; }

        public bool IsEmpty => Type == null && Result == null;

        public static readonly ProcessorResult Empty = new();
    }
}