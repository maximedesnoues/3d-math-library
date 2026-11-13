using System.Numerics;

namespace Maths_Matrices;

public static class MatrixElementaryOperations
{
    public static void SwapLines<T>(Matrix<T> matrix, int line1, int line2) where T : INumber<T>, IMinMaxValue<T>
    {
        for (int j = 0; j < matrix.NbColumns; j++)
        {
            T temp = matrix[line1, j];
            matrix[line1, j] = matrix[line2, j];
            matrix[line2, j] = temp;
        }
    }

    public static void SwapColumns<T>(Matrix<T> matrix, int column1, int column2) where T : INumber<T>, IMinMaxValue<T>
    {
        for (int i = 0; i < matrix.NbLines; i++)
        {
            T temp = matrix[i, column1];
            matrix[i, column1] = matrix[i, column2];
            matrix[i, column2] = temp;
        }
    }

    public static void MultiplyLine<T>(Matrix<T> matrix, int line, T scalar) where T : INumber<T>, IMinMaxValue<T>
    {
        if (scalar == T.Zero)
        {
            throw new MatrixScalarZeroException("The scalar cannot be zero.");
        }

        for (int j = 0; j < matrix.NbColumns; j++)
        {
            matrix[line, j] *= scalar;
        }
    }

    public static void MultiplyColumn<T>(Matrix<T> matrix, int column, T scalar) where T : INumber<T>, IMinMaxValue<T>
    {
        if (scalar == T.Zero)
        {
            throw new MatrixScalarZeroException("The scalar cannot be zero.");
        }

        for (int i = 0; i < matrix.NbLines; i++)
        {
            matrix[i, column] *= scalar;
        }
    }

    public static void AddLineToAnother<T>(Matrix<T> matrix, int lineToApply, int lineToModify, T factor) where T : INumber<T>, IMinMaxValue<T>
    {
        for (int j = 0; j < matrix.NbColumns; j++)
        {
            matrix[lineToModify, j] += matrix[lineToApply, j] * factor;
        }
    }

    public static void AddColumnToAnother<T>(Matrix<T> matrix, int columnToApply, int columnToModify, T factor) where T : INumber<T>, IMinMaxValue<T>
    {
        for (int i = 0; i < matrix.NbLines; i++)
        {
            matrix[i, columnToModify] += matrix[i, columnToApply] * factor;
        }
    }
}