using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public float Health { private set; get; }
    public string Name { private set; get; }
    
    private void Awake()
    {
        Health = 1f;
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    public virtual void Heal(float health)
    {
        health += health;
        Mathf.Clamp(health, 0, 1);
    }

    public virtual void Damage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Kill();
        }
    }

    public void PositionOnTile(Tile tile)
    {
        transform.position = tile.PositionInWorld + new Vector3(tile.TileSize / 2, tile.MeanElevationLevel, tile.TileSize / 2);
    }

    public virtual Unit Spawn(Tile spawnTile)
    {
        

        return this;
    }
}
