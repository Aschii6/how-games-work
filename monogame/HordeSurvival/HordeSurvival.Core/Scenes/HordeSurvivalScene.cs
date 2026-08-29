using System.Collections.Generic;
using HordeSurvival.Core.Components;
using HordeSurvival.Core.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Scenes;

public class HordeSurvivalScene
{
    private Player _player;
    private readonly List<Goblin> _goblins = [];
    private readonly List<GrassTile> _grass = [];
    private readonly List<Entity> _decorations = [];

    public void LoadContent(ContentManager content)
    {
        for (var y = 0; y < 720; y += GrassTile.Size)
        for (var x = 0; x < 1280; x += GrassTile.Size)
            _grass.Add(new GrassTile(content, new Vector2(x + GrassTile.Size / 2f, y + GrassTile.Size / 2f)));

        _decorations.Add(new Rock(content, "Rock1", new Vector2(100, 300)));
        _decorations.Add(new Rock(content, "Rock1", new Vector2(400, 600)));
        _decorations.Add(new Rock(content, "Rock1", new Vector2(700, 400)));
        _decorations.Add(new Rock(content, "Rock2", new Vector2(250, 500)));
        _decorations.Add(new Rock(content, "Rock2", new Vector2(550, 250)));
        _decorations.Add(new Rock(content, "Rock2", new Vector2(1000, 650)));

        _decorations.Add(new Bush(content, new Vector2(350, 450)));
        _decorations.Add(new Bush(content, new Vector2(550, 350)));
        _decorations.Add(new Bush(content, new Vector2(1100, 500)));

        _player = new Player(content);
        _player.GetComponent<TransformComponent>().Position = new Vector2(96, 96);
    }

    public void Update(GameTime gameTime)
    {
        _player.Update(gameTime);
        _decorations.ForEach(decoration => decoration.Update(gameTime));
        _goblins.ForEach(goblin => goblin.Update(gameTime));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _grass.ForEach(tile => tile.Draw(spriteBatch));
        _decorations.ForEach(decoration => decoration.Draw(spriteBatch));
        _player.Draw(spriteBatch);
        _goblins.ForEach(goblin => goblin.Draw(spriteBatch));
    }
}
