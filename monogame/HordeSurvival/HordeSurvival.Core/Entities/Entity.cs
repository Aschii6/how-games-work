using System.Collections.Generic;
using System.Linq;
using HordeSurvival.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Entities;

public class Entity
{
    private readonly List<IComponent> _components = [];

    public T AddComponent<T>(T component) where T : IComponent
    {
        component.Owner = this;
        _components.Add(component);
        return component;
    }

    public T GetComponent<T>() where T : IComponent
        => _components.OfType<T>().FirstOrDefault();

    public void Update(GameTime gameTime)
    {
        foreach (var component in _components.OfType<ComponentBase>())
            component.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in _components.OfType<ComponentBase>())
            component.Draw(spriteBatch);
    }
}
