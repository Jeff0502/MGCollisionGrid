
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

struct Cell
{
    public List<RectCollider> colliders;

    public Cell()
    {
        colliders = new List<RectCollider>();
    }
    
    // Inserting into Cells
    public void Insert(RectCollider collider)
    {
        colliders.Add(collider);
    }

    // Deleting from Cells
    public void Delete(RectCollider collider)
    {
        colliders.Remove(collider);
    }

    // Updating colliders within cells
    public void Update()
    {
        for(int i = 0; i < colliders.Count; i++)
        {
            colliders[i].Update();

            for(int j = 0; j < colliders.Count; j++)
            {
                if(i == j)
                    continue;

                if(colliders[i].Intersects(colliders[j]))
                    colliders[i].isColliding = true;
            }
        }
    }

    // For rectColliders within adjacent cells
    public void CheckCollision(RectCollider rectA)
    {
        foreach(RectCollider rectB in colliders)
        {
            if(rectB.Intersects(rectA))
            {
                rectA.isColliding = true;
                rectB.isColliding = true;
            }
        }
    }

    public void CheckCellCollision(Cell cellA)
    {
        foreach(var colliderA in cellA.colliders)
        {
            CheckCollision(colliderA);
        }
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        foreach(var collider in colliders)
        {
            collider.Draw(_spriteBatch);
        }
    }
}