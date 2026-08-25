using System.Collections.Generic;
using HordeSurvival.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HordeSurvival.Core.Entities;

public class Entity
{
    private readonly List<ComponentBase> _components = [];

    public T AddComponent<T>(T component) where T : ComponentBase
    {
        component.Owner = this;
        _components.Add(component);
        return component;
    }

    public T GetComponent<T>() where T : ComponentBase
    {
        foreach (var component in _components)
            if (component is T match)
                return match;

        return null;
    }

    public void Update(GameTime gameTime)
    {
        foreach (var component in _components)
            component.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in _components)
            component.Draw(spriteBatch);
    }
}
