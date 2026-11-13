namespace Maths_Matrices;

public class MatrixScalarZeroException : Exception
{
    public MatrixScalarZeroException()
    {
    }

    public MatrixScalarZeroException(string message) : base(message)
    {
    }
}