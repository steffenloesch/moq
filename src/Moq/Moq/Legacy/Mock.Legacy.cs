using System;

/// <summary>
/// Legacy class to provide custom error and codefix support.
/// </summary>
public class Mock<T> where T : class
{
    /// <summary>
    /// Legacy constructor to support portability to new API.
    /// </summary>
    [Obsolete("Please run the Moq migration code fix.", true)]
    public Mock()
    {
        throw new NotImplementedException("This Moq4 API is not supported any more. Please run the Moq migration code fix.");
    }
}