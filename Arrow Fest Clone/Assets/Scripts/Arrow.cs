using UnityEngine;

public class Arrow : Pool
{
    public int orbit;
    public Vector3 direction;
    public Vector3 Scale;

    public void SetValues(int orbit)
    {
        this.orbit = orbit;
        direction = transform.localPosition;
        Scale = transform.localScale;
    }

    public void SetLocation(float x, float y)
    {
        Vector3 targetPos = new Vector3(direction.x * x, direction.y * y, 0);
    }
    public void SetScale(float x, float y)
    {
        transform.localScale = new Vector3(x, y, Scale.z);
    }
}
