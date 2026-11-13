namespace Maths_Matrices;

public class Transform
{
    private Transform? _parent;
    private readonly List<Transform> _children = new List<Transform>();

    private Vector3 _localPosition;
    private Vector3 _localRotation;
    private Vector3 _localScale;
    private Vector3 _worldPosition;

    public Vector3 LocalPosition
    {
        get
        {
            return _localPosition;
        }

        set
        {
            _localPosition = value;
            UpdateLocalTranslationMatrix();
        }
    }

    public Vector3 LocalRotation
    {
        get
        {
            return _localRotation;
        }

        set
        {
            _localRotation = value;
            UpdateLocalRotationMatrices();
        }
    }

    public Quaternion LocalRotationQuaternion
    {
        get
        {
            return Quaternion.Euler(_localRotation.x, _localRotation.y, _localRotation.z);
        }

        set
        {
            LocalRotation = value.EulerAngles;
        }
    }

    public Vector3 LocalScale
    {
        get
        {
            return _localScale;
        }

        set
        {
            _localScale = value;
            UpdateLocalScaleMatrix();
        }
    }

    public Vector3 WorldPosition
    {
        get
        {
            return _worldPosition;
        }

        set
        {
            if (_parent == null)
            {
                LocalPosition = value;
            }
            else
            {
                Vector4 worldPosition = new Vector4(value.x, value.y, value.z, 1f);
                Vector4 localPosition = _parent.WorldToLocalMatrix * worldPosition;
                LocalPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z);
            }
        }
    }

    public Matrix<float> LocalTranslationMatrix { get; private set; }

    public Matrix<float> LocalRotationXMatrix { get; private set; }
    public Matrix<float> LocalRotationYMatrix { get; private set; }
    public Matrix<float> LocalRotationZMatrix { get; private set; }
    public Matrix<float> LocalRotationMatrix { get; private set; }

    public Matrix<float> LocalScaleMatrix { get; private set; }

    public Matrix<float> LocalToWorldMatrix { get; private set; }
    public Matrix<float> WorldToLocalMatrix { get; private set; }

    public Transform()
    {
        _localPosition = new Vector3(0f, 0f, 0f);
        _localRotation = new Vector3(0f, 0f, 0f);
        _localScale = new Vector3(1f, 1f, 1f);
        _worldPosition = new Vector3(0f, 0f, 0f);

        LocalTranslationMatrix = Matrix<float>.Identity(4);

        LocalRotationXMatrix = Matrix<float>.Identity(4);
        LocalRotationYMatrix = Matrix<float>.Identity(4);
        LocalRotationZMatrix = Matrix<float>.Identity(4);
        LocalRotationMatrix = Matrix<float>.Identity(4);

        LocalScaleMatrix = Matrix<float>.Identity(4);

        LocalToWorldMatrix = Matrix<float>.Identity(4);
        WorldToLocalMatrix = Matrix<float>.Identity(4);
    }

    public void SetParent(Transform parent)
    {
        if (_parent != null)
        {
            _parent._children.Remove(this);
        }

        _parent = parent;
        _parent._children.Add(this);

        UpdateTransformationMatrices();
    }

    private void UpdateLocalTranslationMatrix()
    {
        LocalTranslationMatrix = new Matrix<float>(new[,]
        {
            { 1f, 0f, 0f, _localPosition.x },
            { 0f, 1f, 0f, _localPosition.y },
            { 0f, 0f, 1f, _localPosition.z },
            { 0f, 0f, 0f, 1f }
        });

        UpdateTransformationMatrices();
    }

    private void UpdateLocalRotationMatrices()
    {
        float rotationXInRadians = DegreesToRadians(_localRotation.x);
        float rotationYInRadians = DegreesToRadians(_localRotation.y);
        float rotationZInRadians = DegreesToRadians(_localRotation.z);

        float cosX = MathF.Cos(rotationXInRadians);
        float sinX = MathF.Sin(rotationXInRadians);

        float cosY = MathF.Cos(rotationYInRadians);
        float sinY = MathF.Sin(rotationYInRadians);

        float cosZ = MathF.Cos(rotationZInRadians);
        float sinZ = MathF.Sin(rotationZInRadians);

        LocalRotationXMatrix = new Matrix<float>(new[,]
        {
            { 1f, 0f, 0f, 0f },
            { 0f, cosX, -sinX, 0f },
            { 0f, sinX, cosX, 0f },
            { 0f, 0f, 0f, 1f }
        });

        LocalRotationYMatrix = new Matrix<float>(new[,]
        {
            { cosY, 0f, sinY, 0f },
            { 0f, 1f, 0f, 0f },
            { -sinY, 0f, cosY, 0f },
            { 0f, 0f, 0f, 1f }
        });

        LocalRotationZMatrix = new Matrix<float>(new[,]
        {
            { cosZ, -sinZ, 0f, 0f },
            { sinZ, cosZ, 0f, 0f },
            { 0f, 0f, 1f, 0f },
            { 0f, 0f, 0f, 1f }
        });

        LocalRotationMatrix = LocalRotationYMatrix * LocalRotationXMatrix * LocalRotationZMatrix;

        UpdateTransformationMatrices();
    }

    private void UpdateLocalScaleMatrix()
    {
        LocalScaleMatrix = new Matrix<float>(new[,]
        {
            { _localScale.x, 0f, 0f, 0f },
            { 0f, _localScale.y, 0f, 0f },
            { 0f, 0f, _localScale.z, 0f },
            { 0f, 0f, 0f, 1f }
        });

        UpdateTransformationMatrices();
    }

    private void UpdateTransformationMatrices()
    {
        Matrix<float> localMatrix = LocalTranslationMatrix * LocalRotationMatrix * LocalScaleMatrix;

        if (_parent == null)
        {
            LocalToWorldMatrix = localMatrix;
        }
        else
        {
            LocalToWorldMatrix = _parent.LocalToWorldMatrix * localMatrix;
        }

        WorldToLocalMatrix = LocalToWorldMatrix.InvertByDeterminant();

        _worldPosition = new Vector3(LocalToWorldMatrix[0, 3], LocalToWorldMatrix[1, 3], LocalToWorldMatrix[2, 3]);

        foreach (Transform child in _children)
        {
            child.UpdateTransformationMatrices();
        }
    }

    private static float DegreesToRadians(float degrees)
    {
        return degrees * MathF.PI / 180f;
    }
}