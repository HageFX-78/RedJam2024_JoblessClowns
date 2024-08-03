using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropType
{
    Trigger,
    Normal
}

public enum ColliderType
{
    Box,
    Circle
}
[CreateAssetMenu(fileName = "New Prop", menuName = "Prop")]
public class Prop : ScriptableObject
{
    public new string name;
    public PropType propType;
    public int propBaseCoinValue;

    public Sprite propSprite;

    public Color propColorOverlay = Color.white;
    public ColliderType colliderType;
    public Vector2 propSpriteSize = Vector2.one;
    public float propColliderRadius = 1.0f;
    public Vector2 propColliderSize = Vector2.one;
}
