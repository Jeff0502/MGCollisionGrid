using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using CollisionGrid;
struct Grid
{
    // For this naive approach, do not make a collider > cell_size
    private const int CELL_SIZE = 20;

    private int cellsX, cellsY;

    private Cell[,] cells;

    // Map width and height are multiples of cell_size
    public Grid(int MAP_WIDTH, int MAP_HEIGHT)
    {
        cellsX = MAP_WIDTH / CELL_SIZE;
        cellsY = MAP_HEIGHT / CELL_SIZE;

        cells = new Cell[cellsX, cellsY];

        for(int i = 0; i < cellsX; i++)
        {
            for(int j = 0; j < cellsY; j++)
            {
                cells[i, j] = new Cell();
            }
        }
    }

    public void Insert(RectCollider collider)
    {
        int i = (int)collider.X / CELL_SIZE;
        int j = (int)collider.Y / CELL_SIZE;

        cells[i, j].Insert(collider);
    }

    // Explain why we used indices here
    public void Delete(RectCollider collider, int i, int j)
    {
        cells[i, j].Delete(collider);
    }

    public void Move(RectCollider collider, int deltaX, int deltaY)
    {
        int oldCellX = (int)collider.X / CELL_SIZE;
        int oldCellY = (int)collider.Y / CELL_SIZE;

        int newCellX = (int)(collider.X + deltaX) / CELL_SIZE;
        int newCellY = (int)(collider.Y + deltaY) / CELL_SIZE;

        if(collider.X + collider.Width + deltaX < Game1.MAP_WIDTH && collider.X + deltaX >= 0)
            collider.X += deltaX;
        if(collider.Y + collider.Height + deltaY < Game1.MAP_HEIGHT && collider.Y + deltaY >= 0)
            collider.Y += deltaY;

        if(newCellX  == oldCellX && newCellY == oldCellY)
            return;

        else
        {
            Delete(collider, oldCellX, oldCellY);
            Insert(collider);
        }
    }

    public void Update()
    {
        for(int i = 0; i < cellsX; i++)
        {
            for(int j = 0; j < cellsY; j++)
            {
                cells[i, j].Update();
                CheckSurrounding(i, j);   
            }
        }
    }

    public void CheckSurrounding(int x, int y)
    {
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j<= 1; j++)
            {
                if(i == 0 && j == 0)
                    continue;
                
                if(x + i >= cellsX || y + j >= cellsY || x + i < 0 || y + j < 0)
                    continue;

                cells[x, y].CheckCellCollision(cells[x + i, y + j]);
            }
        }
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        foreach(var cell in cells)
            cell.Draw(_spriteBatch);

        for(int i = 0; i < Game1.MAP_WIDTH; i += CELL_SIZE)
            _spriteBatch.DrawLine(i, 0, i, Game1.MAP_HEIGHT, Color.Green);

        for(int i = 0; i < Game1.MAP_HEIGHT; i += CELL_SIZE)
            _spriteBatch.DrawLine(0, i, Game1.MAP_WIDTH, i, Color.Green);
    }
}