using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : Singleton<ItemAssets>
{
    private void Awake()
    {
        InitSingleton(this);
    }

    public Sprite XPipeSprite;
    public Sprite TPipeSprite;
    public Sprite IPipeSprite;
    public Sprite LPipeSprite;

}
