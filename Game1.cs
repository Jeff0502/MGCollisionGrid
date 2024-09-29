using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using MonoGame.Extended;

namespace CollisionGrid;

public class Game1 : Game
{
    public static readonly int MAP_WIDTH = 800, MAP_HEIGHT = 480;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private List<RectCollider> rects = new List<RectCollider>();

    private Grid collisionGrid;

    private SimpleFps fps = new SimpleFps();

    private SpriteFont font;

    private RectCollider player;

    private bool useGrid = false;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Change for more or less rects
        uint rectCount = 500;

        if(useGrid)
            InitializeGrid(rectCount);
        else
            InitializeRects(rectCount);

        base.Initialize();
    }

    private void InitializeGrid(uint rectCount)
    {
        collisionGrid = new Grid(MAP_WIDTH, MAP_HEIGHT);

        Random random = new Random();

        player = new RectCollider(10, 10, 20, 20);

        collisionGrid.Insert(player);

        for(int i = 0; i < rectCount; i++)
        {
            RectCollider collider = new RectCollider(random.Next(0, MAP_WIDTH - 20), random.Next(0, MAP_HEIGHT - 20), random.Next(5, 18), random.Next(5, 18));

            collisionGrid.Insert(collider);
        }
    }

    private void InitializeRects(uint rectCount)
    {
        Random random = new Random();

        player = new RectCollider(10, 10, 20, 20);

        rects.Add(player);

        for(int i = 0; i < rectCount; i++)
        {
            RectCollider collider = new RectCollider(random.Next(0, MAP_WIDTH - 20), random.Next(0, MAP_HEIGHT - 20), random.Next(5, 18), random.Next(5, 18));

            rects.Add(collider);
        }
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        font = Content.Load<SpriteFont>("Font");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        fps.Update(gameTime);

        if(useGrid)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.D))
                collisionGrid.Move(player, 1, 0);
            if(Keyboard.GetState().IsKeyDown(Keys.A))
                collisionGrid.Move(player, 0, -1);
            if(Keyboard.GetState().IsKeyDown(Keys.W))
                collisionGrid.Move(player, 0, -1);
            if(Keyboard.GetState().IsKeyDown(Keys.S))
                collisionGrid.Move(player, 0, 1);

            collisionGrid.Update();
        }

        else 
        {
            if(Keyboard.GetState().IsKeyDown(Keys.D))
                player.X += 1;
            if(Keyboard.GetState().IsKeyDown(Keys.A))
                player.X -= 1;
            if(Keyboard.GetState().IsKeyDown(Keys.W))
                player.Y -= 1;
            if(Keyboard.GetState().IsKeyDown(Keys.S))
                player.Y += 1;

            UpdateRects();
        }

        base.Update(gameTime);
    }

    private void UpdateRects()
    {

        foreach(var rect in rects)
        {
            rect.Update();
        }

        for(int i = 0; i < rects.Count; i++)
        {
            for(int j = 0; j < rects.Count; j++)
            {
                if(i == j)
                    continue;
                
                if(rects[i].Intersects(rects[j]))
                {
                    rects[i].isColliding = true;
                    rects[j].isColliding = true;
                }
            }
        }
    }

    private void DrawRects(SpriteBatch _spriteBatch)
    {
       
        foreach(var rect in rects)
        {
            rect.Draw(_spriteBatch);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
      
        if(useGrid)
            collisionGrid.Draw(_spriteBatch);

        else 
            DrawRects(_spriteBatch);

        _spriteBatch.FillRectangle(new Rectangle(600, 0, 200, 100), Color.White);
        fps.DrawFps(_spriteBatch, font, new Vector2(600, 0), Color.Blue);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

// PLEASE NOTE: The following class has been written by willmotil (https://github.com/willmotil)
public class SimpleFps
{
    private double frames = 0;
    private double updates = 0;
    private double elapsed = 0;
    private double last = 0;
    private double now = 0;
    public double msgFrequency = 1.0f; 
    public string msg = "";

    /// <summary>
    /// The msgFrequency here is the reporting time to update the message.
    /// </summary>
    public void Update(GameTime gameTime)
    {
        now = gameTime.TotalGameTime.TotalSeconds;
        elapsed = (double)(now - last);
        if (elapsed > msgFrequency)
        {
            msg = " Fps: " + Math.Round((frames / elapsed), 2).ToString() + "\n Elapsed time: " + Math.Round(elapsed, 2).ToString() +  "\n Updates: " + updates.ToString() + "\n Frames: " + frames.ToString();
            //Console.WriteLine(msg);
            elapsed = 0;
            frames = 0;
            updates = 0;
            last = now;
        }
        updates++;
    }

    public void DrawFps(SpriteBatch spriteBatch, SpriteFont font, Vector2 fpsDisplayPosition, Color fpsTextColor)
    {
        spriteBatch.DrawString(font, msg, fpsDisplayPosition, fpsTextColor);
        frames++;
    }
}