using System.Numerics;

namespace Maths_Matrices;

public static class MatrixRowReductionAlgorithm
{
    public static (Matrix<T>, Matrix<T>) Apply<T>(Matrix<T> matrix1, Matrix<T> matrix2, bool throwExceptionIfNoPivot = false) where T : INumber<T>, IMinMaxValue<T>
    {
        Matrix<T> augmentedMatrix = Matrix<T>.GenerateAugmentedMatrix(matrix1, matrix2);

        int pivotLine = 0;

        for (int pivotColumn = 0; pivotColumn < matrix1.NbColumns && pivotLine < augmentedMatrix.NbLines; pivotColumn++)
        {
            int bestPivotLine = FindBestPivotLine(augmentedMatrix, pivotLine, pivotColumn);

            if (bestPivotLine == -1)
            {
                if (throwExceptionIfNoPivot)
                {
                    throw new MatrixRowReductionException("Unable to find a non-zero pivot.");
                }

                continue;
            }

            if (bestPivotLine != pivotLine)
            {
                MatrixElementaryOperations.SwapLines(augmentedMatrix, bestPivotLine, pivotLine);
            }

            T pivot = augmentedMatrix[pivotLine, pivotColumn];

            if (pivot != T.One)
            {
                MatrixElementaryOperations.MultiplyLine(augmentedMatrix, pivotLine, T.One / pivot);
            }

            for (int line = 0; line < augmentedMatrix.NbLines; line++)
            {
                if (line == pivotLine)
                {
                    continue;
                }

                T factor = -augmentedMatrix[line, pivotColumn];

                if (factor != T.Zero)
                {
                    MatrixElementaryOperations.AddLineToAnother(augmentedMatrix, pivotLine, line, factor);
                }
            }

            pivotLine++;
        }

        return augmentedMatrix.Split(matrix1.NbColumns - 1);
    }

    private static int FindBestPivotLine<T>(Matrix<T> matrix, int startLine, int column) where T : INumber<T>, IMinMaxValue<T>
    {
        T maxValue = T.MinValue;
        int bestPivotLine = -1;

        for (int line = startLine; line < matrix.NbLines; line++)
        {
            T value = matrix[line, column];

            if (value != T.Zero && value > maxValue)
            {
                maxValue = value;
                bestPivotLine = line;
            }
        }

        return bestPivotLine;
    }
}