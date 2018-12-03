using System;
using System.Runtime.Serialization;

namespace Renamer.Exceptions
{
    // Summary:
    //     The exception that is thrown when a method call is invalid for the object's
    //     current state.
    /// <summary>
    /// The exception that is thrown when a convert file method call is not available due to difference between OptionStates;
    /// </summary>
    [Serializable]
    public class DifferentOptionState : SystemException
    {
        // Summary:
        //     Initializes a new instance of the System.DifferentOptionState class.
        public DifferentOptionState() : base() { }
        //
        // Summary:
        //     Initializes a new instance of the System.DifferentOptionState class
        //     with a specified error message.
        //
        // Parameters:
        //   message:
        //     The message that describes the error.
        public DifferentOptionState(string message) : base(message) { }
        //
        // Summary:
        //     Initializes a new instance of the System.DifferentOptionState class
        //     with serialized data.
        //
        // Parameters:
        //   info:
        //     The object that holds the serialized object data.
        //
        //   context:
        //     The contextual information about the source or destination.
        protected DifferentOptionState(SerializationInfo info, StreamingContext context) : base(info, context) { }
        //
        // Summary:
        //     Initializes a new instance of the System.DifferentOptionState class
        //     with a specified error message and a reference to the inner exception that
        //     is the cause of this exception.
        //
        // Parameters:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception. If the innerException
        //     parameter is not a null reference (Nothing in Visual Basic), the current
        //     exception is raised in a catch block that handles the inner exception.
        public DifferentOptionState(string message, Exception innerException) : base(message, innerException) { }
    }
}
