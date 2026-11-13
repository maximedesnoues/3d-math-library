namespace Maths_Matrices;

public struct Quaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public Quaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static Quaternion Identity
    {
        get
        {
            return new Quaternion(0f, 0f, 0f, 1f);
        }
    }

    public Matrix<float> Matrix
    {
        get
        {
            float xx = x * x;
            float yy = y * y;
            float zz = z * z;

            float xy = x * y;
            float xz = x * z;
            float yz = y * z;

            float xw = x * w;
            float yw = y * w;
            float zw = z * w;

            return new Matrix<float>(new[,]
            {
                { 1f - 2f * (yy + zz), 2f * (xy - zw), 2f * (xz + yw), 0f },
                { 2f * (xy + zw), 1f - 2f * (xx + zz), 2f * (yz - xw), 0f },
                { 2f * (xz - yw), 2f * (yz + xw), 1f - 2f * (xx + yy), 0f },
                { 0f, 0f, 0f, 1f }
            });
        }
    }

    public Vector3 EulerAngles
    {
        get
        {
            Matrix<float> rotationMatrix = Matrix;

            float sinX = -rotationMatrix[1, 2];
            sinX = MathF.Max(-1f, MathF.Min(1f, sinX));

            float rotationXInRadians = MathF.Asin(sinX);
            float rotationYInRadians = MathF.Atan2(rotationMatrix[0, 2], rotationMatrix[2, 2]);
            float rotationZInRadians = MathF.Atan2(rotationMatrix[1, 0], rotationMatrix[1, 1]);

            float rotationXInDegrees = RadiansToDegrees(rotationXInRadians);
            float rotationYInDegrees = RadiansToDegrees(rotationYInRadians);
            float rotationZInDegrees = RadiansToDegrees(rotationZInRadians);

            return new Vector3(rotationXInDegrees, rotationYInDegrees, rotationZInDegrees);
        }
    }

    public static Quaternion AngleAxis(float angle, Vector3 axis)
    {
        float axisMagnitude = MathF.Sqrt(axis.x * axis.x + axis.y * axis.y + axis.z * axis.z);

        float normalizedAxisX = axis.x / axisMagnitude;
        float normalizedAxisY = axis.y / axisMagnitude;
        float normalizedAxisZ = axis.z / axisMagnitude;

        float halfAngleInRadians = DegreesToRadians(angle) / 2f;

        float sinHalfAngle = MathF.Sin(halfAngleInRadians);
        float cosHalfAngle = MathF.Cos(halfAngleInRadians);

        return new Quaternion(normalizedAxisX * sinHalfAngle, normalizedAxisY * sinHalfAngle, normalizedAxisZ * sinHalfAngle, cosHalfAngle);
    }

    public static Quaternion Euler(float x, float y, float z)
    {
        Quaternion rotationXAxis = AngleAxis(x, new Vector3(1f, 0f, 0f));
        Quaternion rotationYAxis = AngleAxis(y, new Vector3(0f, 1f, 0f));
        Quaternion rotationZAxis = AngleAxis(z, new Vector3(0f, 0f, 1f));

        return rotationYAxis * rotationXAxis * rotationZAxis;
    }

    public static Quaternion operator *(Quaternion quaternion1, Quaternion quaternion2)
    {
        float x = quaternion1.w * quaternion2.x + quaternion1.x * quaternion2.w + quaternion1.y * quaternion2.z - quaternion1.z * quaternion2.y;
        float y = quaternion1.w * quaternion2.y - quaternion1.x * quaternion2.z + quaternion1.y * quaternion2.w + quaternion1.z * quaternion2.x;
        float z = quaternion1.w * quaternion2.z + quaternion1.x * quaternion2.y - quaternion1.y * quaternion2.x + quaternion1.z * quaternion2.w;
        float w = quaternion1.w * quaternion2.w - quaternion1.x * quaternion2.x - quaternion1.y * quaternion2.y - quaternion1.z * quaternion2.z;

        return new Quaternion(x, y, z, w);
    }

    public static Vector3 operator *(Quaternion quaternion, Vector3 vector)
    {
        Quaternion vectorQuaternion = new Quaternion(vector.x, vector.y, vector.z, 0f);
        Quaternion inverseQuaternion = new Quaternion(-quaternion.x, -quaternion.y, -quaternion.z, quaternion.w);

        Quaternion rotatedVectorQuaternion = quaternion * vectorQuaternion * inverseQuaternion;

        return new Vector3(rotatedVectorQuaternion.x, rotatedVectorQuaternion.y, rotatedVectorQuaternion.z);
    }

    private static float DegreesToRadians(float degrees)
    {
        return degrees * MathF.PI / 180f;
    }

    private static float RadiansToDegrees(float radians)
    {
        return radians * 180f / MathF.PI;
    }
}