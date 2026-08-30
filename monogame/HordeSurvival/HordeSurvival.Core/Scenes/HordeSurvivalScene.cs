using System;
using System.Collections.Generic;
using HordeSurvival.Core.Combat;
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

    private uint _wave = 0;
    private bool _waveIncoming = true;
    private float _waveTimer = 5;
    private readonly Random _random = new();
    private ContentManager _content;
    private SpriteFont _font;

    public event Action GameOver;

    public void LoadContent(ContentManager content)
    {
        _content = content;
        _font = content.Load<SpriteFont>("Fonts/Fredoka");

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
        _player.Hurtbox.Died += OnPlayerDied;
    }

    private void OnPlayerDied() => GameOver?.Invoke();

    private void SpawnWave()
    {
        _wave++;
        int min = (int)(_wave * 1.5) + 1;
        int max = min + 2;
        var count = _random.Next(min, max + 1);

        for (int i = 0; i < count; i++)
        {
            var goblin = new Goblin(_content, _player);
            goblin.GetComponent<TransformComponent>().Position =
                new Vector2(_random.Next(0, 1280), _random.Next(720, 1200));
            goblin.Died += OnGoblinDied;
            _goblins.Add(goblin);
        }

        _waveIncoming = false;
    }

    private void OnGoblinDied(Goblin goblin) => _goblins.Remove(goblin);

    public void Update(GameTime gameTime)
    {
        if (_waveIncoming)
        {
            _waveTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_waveTimer <= 0)
                SpawnWave();
        }
        else if (_goblins.Count == 0)
        {
            _waveIncoming = true;
            _waveTimer = 15;
        }

        _player.Update(gameTime);
        _decorations.ForEach(decoration => decoration.Update(gameTime));

        _goblins.ForEach(goblin => goblin.Update(gameTime));

        ResolveCombat();
    }

    private void ResolveCombat()
    {
        foreach (var goblin in _goblins.ToArray())
        {
            CombatSystem.TryHit(_player.Hitbox, goblin.Hurtbox);
            CombatSystem.TryHit(goblin.Hitbox, _player.Hurtbox);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _grass.ForEach(tile => tile.Draw(spriteBatch));
        _decorations.ForEach(decoration => decoration.Draw(spriteBatch));
        _player.Draw(spriteBatch);
        _goblins.ForEach(goblin => goblin.Draw(spriteBatch));

        DrawHud(spriteBatch);
    }

    private void DrawHud(SpriteBatch spriteBatch)
    {
        var hpText = $"HP: {(int)_player.Hurtbox.Hp}";
        spriteBatch.DrawString(_font, hpText, new Vector2(20, 20), Color.White, 0f, Vector2.Zero, 0.5f,
            SpriteEffects.None, 0f);
    }
}
