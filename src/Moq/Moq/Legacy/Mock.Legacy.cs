using System;

public class Mock<T> where T : class
{
    [Obsolete("Please run the Moq migration code fix.", true)]
    public Mock()
    {
        throw new NotImplementedException("This Moq4 API is not supported any more. Please run the Moq migration code fix.");
    }
}