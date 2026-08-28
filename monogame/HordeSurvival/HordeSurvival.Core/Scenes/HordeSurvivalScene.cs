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

    public void LoadContent(ContentManager content)
    {
        _player = new Player(content);
        _player.GetComponent<TransformComponent>().Position = new Vector2(96, 96);

        var goblin = new Goblin(content, _player);
        goblin.GetComponent<TransformComponent>().Position = new Vector2(400, 600);
        _goblins.Add(goblin);
    }

    public void Update(GameTime gameTime)
    {
        _player.Update(gameTime);
        _goblins.ForEach(goblin => goblin.Update(gameTime));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _player.Draw(spriteBatch);
        _goblins.ForEach(goblin => goblin.Draw(spriteBatch));
    }
}
