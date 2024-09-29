
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

class RectCollider
{
    public RectangleF rect;

    public bool isColliding;

    public float X
    {
        get => rect.X;
        set => rect.X = value;
    }

    public float Y 
    {
        get => rect.Y;
        set => rect.Y = value;
    }

    public float Width
    {
        get => rect.Width;
        set => rect.Width = value;
    }

    public float Height
    {
        get => rect.Height;
        set => rect.Height = value;
    }

    public RectCollider(int x, int y, int w, int h)
    {
        rect = new RectangleF(x, y, w, h);
        isColliding = false;
    }

    public void Update()
    {
        isColliding = false;
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        if(!isColliding)
            _spriteBatch.FillRectangle(rect, Color.White);
        else
            _spriteBatch.FillRectangle(rect, Color.Red);
    }

    public bool Intersects(RectCollider rectB) => rect.Intersects(rectB.rect);
}