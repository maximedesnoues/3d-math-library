namespace Maths_Matrices;

public class Vector4
{
    public float x { get; }
    public float y { get; }
    public float z { get; }
    public float w { get; }

    public Vector4(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static implicit operator Matrix<float>(Vector4 vector)
    {
        return new Matrix<float>(new[,]
        {
            { vector.x },
            { vector.y },
            { vector.z },
            { vector.w }
        });
    }

    public static implicit operator Vector4(Matrix<float> matrix)
    {
        if (matrix.NbLines != 4 || matrix.NbColumns != 1)
        {
            throw new ArgumentException("The matrix must have 4 lines and 1 column to be converted into a Vector4.");
        }

        return new Vector4(matrix[0, 0], matrix[1, 0], matrix[2, 0], matrix[3, 0]);
    }
}