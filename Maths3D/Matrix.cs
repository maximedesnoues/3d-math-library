using System.Numerics;

namespace Maths_Matrices;

public class Matrix<T> where T : INumber<T>, IMinMaxValue<T>
{
    private readonly T[,] _values;

    public int NbLines { get; }
    public int NbColumns { get; }

    public Matrix(int nbLines, int nbColumns)
    {
        NbLines = nbLines;
        NbColumns = nbColumns;
        _values = new T[nbLines, nbColumns];
    }

    public Matrix(T[,] values)
    {
        NbLines = values.GetLength(0);
        NbColumns = values.GetLength(1);
        _values = new T[NbLines, NbColumns];

        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                _values[i, j] = values[i, j];
            }
        }
    }

    public Matrix(Matrix<T> matrix)
    {
        NbLines = matrix.NbLines;
        NbColumns = matrix.NbColumns;
        _values = new T[NbLines, NbColumns];

        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                _values[i, j] = matrix[i, j];
            }
        }
    }

    public T this[int line, int column]
    {
        get
        {
            return _values[line, column];
        }

        set
        {
            _values[line, column] = value;
        }
    }

    public T[,] ToArray2D()
    {
        T[,] resultArray = new T[NbLines, NbColumns];

        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                resultArray[i, j] = _values[i, j];
            }
        }

        return resultArray;
    }

    public bool IsIdentity()
    {
        if (NbLines != NbColumns)
        {
            return false;
        }

        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                if (i == j && this[i, j] != T.One)
                {
                    return false;
                }

                if (i != j && this[i, j] != T.Zero)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void Add(Matrix<T> matrix)
    {
        if (NbLines != matrix.NbLines || NbColumns != matrix.NbColumns)
        {
            throw new MatrixSumException("Matrices must have the same dimensions.");
        }

        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                _values[i, j] += matrix[i, j];
            }
        }
    }

    public void Multiply(T scalar)
    {
        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                _values[i, j] *= scalar;
            }
        }
    }

    public Matrix<T> Multiply(Matrix<T> matrix)
    {
        return Multiply(this, matrix);
    }

    public Matrix<T> Transpose()
    {
        return Transpose(this);
    }

    public (Matrix<T>, Matrix<T>) Split(int columnIndex)
    {
        Matrix<T> leftMatrix = new Matrix<T>(NbLines, columnIndex + 1);
        Matrix<T> rightMatrix = new Matrix<T>(NbLines, NbColumns - columnIndex - 1);

        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j <= columnIndex; j++)
            {
                leftMatrix[i, j] = this[i, j];
            }

            for (int j = columnIndex + 1; j < NbColumns; j++)
            {
                rightMatrix[i, j - columnIndex - 1] = this[i, j];
            }
        }

        return (leftMatrix, rightMatrix);
    }

    public Matrix<T> SubMatrix(int lineToRemove, int columnToRemove)
    {
        return SubMatrix(this, lineToRemove, columnToRemove);
    }

    public Matrix<T> Adjugate()
    {
        return Adjugate(this);
    }

    public Matrix<T> InvertByRowReduction()
    {
        return InvertByRowReduction(this);
    }

    public Matrix<T> InvertByDeterminant()
    {
        return InvertByDeterminant(this);
    }

    public static Matrix<T> Identity(int size)
    {
        Matrix<T> identityMatrix = new Matrix<T>(size, size);

        for (int i = 0; i < size; i++)
        {
            identityMatrix[i, i] = T.One;
        }

        return identityMatrix;
    }

    public static Matrix<T> Add(Matrix<T> matrix1, Matrix<T> matrix2)
    {
        Matrix<T> resultMatrix = new Matrix<T>(matrix1);

        resultMatrix.Add(matrix2);

        return resultMatrix;
    }

    public static Matrix<T> Multiply(Matrix<T> matrix, T scalar)
    {
        Matrix<T> resultMatrix = new Matrix<T>(matrix);

        resultMatrix.Multiply(scalar);

        return resultMatrix;
    }

    public static Matrix<T> Multiply(Matrix<T> matrix1, Matrix<T> matrix2)
    {
        if (matrix1.NbColumns != matrix2.NbLines)
        {
            throw new MatrixMultiplyException("The number of columns of the first matrix must be equal to the number of lines of the second matrix.");
        }

        Matrix<T> resultMatrix = new Matrix<T>(matrix1.NbLines, matrix2.NbColumns);

        for (int i = 0; i < matrix1.NbLines; i++)
        {
            for (int j = 0; j < matrix2.NbColumns; j++)
            {
                T sum = T.Zero;

                for (int k = 0; k < matrix1.NbColumns; k++)
                {
                    sum += matrix1[i, k] * matrix2[k, j];
                }

                resultMatrix[i, j] = sum;
            }
        }

        return resultMatrix;
    }

    public static Matrix<T> Transpose(Matrix<T> matrix)
    {
        Matrix<T> resultMatrix = new Matrix<T>(matrix.NbColumns, matrix.NbLines);

        for (int i = 0; i < matrix.NbLines; i++)
        {
            for (int j = 0; j < matrix.NbColumns; j++)
            {
                resultMatrix[j, i] = matrix[i, j];
            }
        }

        return resultMatrix;
    }

    public static Matrix<T> GenerateAugmentedMatrix(Matrix<T> matrix1, Matrix<T> matrix2)
    {
        if (matrix1.NbLines != matrix2.NbLines)
        {
            throw new ArgumentException("Matrices must have the same number of lines.");
        }

        Matrix<T> augmentedMatrix = new Matrix<T>(matrix1.NbLines, matrix1.NbColumns + matrix2.NbColumns);

        for (int i = 0; i < matrix1.NbLines; i++)
        {
            for (int j = 0; j < matrix1.NbColumns; j++)
            {
                augmentedMatrix[i, j] = matrix1[i, j];
            }

            for (int j = 0; j < matrix2.NbColumns; j++)
            {
                augmentedMatrix[i, matrix1.NbColumns + j] = matrix2[i, j];
            }
        }

        return augmentedMatrix;
    }

    public static Matrix<T> SubMatrix(Matrix<T> matrix, int lineToRemove, int columnToRemove)
    {
        Matrix<T> subMatrix = new Matrix<T>(matrix.NbLines - 1, matrix.NbColumns - 1);

        int subMatrixLine = 0;

        for (int i = 0; i < matrix.NbLines; i++)
        {
            if (i == lineToRemove)
            {
                continue;
            }

            int subMatrixColumn = 0;

            for (int j = 0; j < matrix.NbColumns; j++)
            {
                if (j == columnToRemove)
                {
                    continue;
                }

                subMatrix[subMatrixLine, subMatrixColumn] = matrix[i, j];
                subMatrixColumn++;
            }

            subMatrixLine++;
        }

        return subMatrix;
    }

    public static T Determinant(Matrix<T> matrix)
    {
        if (matrix.NbLines != matrix.NbColumns)
        {
            throw new InvalidOperationException("The determinant can only be calculated for a square matrix.");
        }

        if (matrix.NbLines == 1)
        {
            return matrix[0, 0];
        }

        if (matrix.NbLines == 2)
        {
            return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
        }

        T determinant = T.Zero;

        for (int j = 0; j < matrix.NbColumns; j++)
        {
            if (matrix[0, j] == T.Zero)
            {
                continue;
            }

            T cofactorSign = j % 2 == 0 ? T.One : -T.One;
            Matrix<T> subMatrix = matrix.SubMatrix(0, j);

            determinant += cofactorSign * matrix[0, j] * Determinant(subMatrix);
        }

        return determinant;
    }

    public static Matrix<T> Adjugate(Matrix<T> matrix)
    {
        if (matrix.NbLines != matrix.NbColumns)
        {
            throw new InvalidOperationException("The adjugate can only be calculated for a square matrix.");
        }

        if (matrix.NbLines == 1)
        {
            return Identity(1);
        }

        Matrix<T> cofactorMatrix = new Matrix<T>(matrix.NbLines, matrix.NbColumns);

        for (int i = 0; i < matrix.NbLines; i++)
        {
            for (int j = 0; j < matrix.NbColumns; j++)
            {
                T cofactorSign = (i + j) % 2 == 0 ? T.One : -T.One;
                Matrix<T> subMatrix = matrix.SubMatrix(i, j);

                cofactorMatrix[i, j] = cofactorSign * Determinant(subMatrix);
            }
        }

        return Transpose(cofactorMatrix);
    }

    public static Matrix<T> InvertByRowReduction(Matrix<T> matrix)
    {
        if (matrix.NbLines != matrix.NbColumns)
        {
            throw new MatrixInvertException("Only square matrices can be inverted.");
        }

        Matrix<T> identityMatrix = Identity(matrix.NbLines);

        try
        {
            var (_, invertedMatrix) = MatrixRowReductionAlgorithm.Apply(matrix, identityMatrix, true);
            return invertedMatrix;
        }
        catch (MatrixRowReductionException)
        {
            throw new MatrixInvertException("The matrix is not invertible.");
        }
    }

    public static Matrix<T> InvertByDeterminant(Matrix<T> matrix)
    {
        if (matrix.NbLines != matrix.NbColumns)
        {
            throw new MatrixInvertException("Only square matrices can be inverted.");
        }

        T determinant = Determinant(matrix);

        if (determinant == T.Zero)
        {
            throw new MatrixInvertException("The matrix is not invertible.");
        }

        Matrix<T> adjugateMatrix = Adjugate(matrix);

        return Multiply(adjugateMatrix, T.One / determinant);
    }

    public static Matrix<T> operator +(Matrix<T> matrix1, Matrix<T> matrix2)
    {
        return Add(matrix1, matrix2);
    }

    public static Matrix<T> operator -(Matrix<T> matrix)
    {
        return Multiply(matrix, -T.One);
    }

    public static Matrix<T> operator -(Matrix<T> matrix1, Matrix<T> matrix2)
    {
        return Add(matrix1, -matrix2);
    }

    public static Matrix<T> operator *(Matrix<T> matrix, T scalar)
    {
        return Multiply(matrix, scalar);
    }

    public static Matrix<T> operator *(T scalar, Matrix<T> matrix)
    {
        return Multiply(matrix, scalar);
    }

    public static Matrix<T> operator *(Matrix<T> matrix1, Matrix<T> matrix2)
    {
        return Multiply(matrix1, matrix2);
    }
}