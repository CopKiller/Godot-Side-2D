namespace Core.Game.Interfaces.Services.Physic.World;

public interface IVector2
{
    
    /// <summary>Round the members of this <see cref="Vector2" /> towards positive infinity.</summary>
    void Ceiling();

    /// <summary>Compares whether current instance is equal to specified <see cref="Object" />.</summary>
    /// <param name="obj">The <see cref="Object" /> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    bool Equals(object obj);

    /// <summary>Compares whether current instance is equal to specified <see cref="Vector2" />.</summary>
    /// <param name="other">The <see cref="Vector2" /> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
    bool Equals(IVector2 other);

    /// <summary>Round the members of this <see cref="Vector2" /> towards negative infinity.</summary>
    void Floor();

    /// <summary>Gets the hash code of this <see cref="Vector2" />.</summary>
    /// <returns>Hash code of this <see cref="Vector2" />.</returns>
    int GetHashCode();

    /// <summary>Returns the length of this <see cref="Vector2" />.</summary>
    /// <returns>The length of this <see cref="Vector2" />.</returns>
    float Length();

    /// <summary>Returns the squared length of this <see cref="Vector2" />.</summary>
    /// <returns>The squared length of this <see cref="Vector2" />.</returns>
    float LengthSquared();

    /// <summary>Turns this <see cref="Vector2" /> to a unit vector with the same direction.</summary>
    void Normalize();

    /// <summary>Round the members of this <see cref="Vector2" /> to the nearest integer value.</summary>
    void Round();

    /// <summary>
    ///     Returns a <see cref="String" /> representation of this <see cref="Vector2" /> in the format: {X:[
    ///     <see cref="Vector2.X" />] Y:[<see cref="Vector2.Y" />]}
    /// </summary>
    /// <returns>A <see cref="String" /> representation of this <see cref="Vector2" />.</returns>
    string ToString();

    /// <summary>Deconstruction method for <see cref="Vector2" />.</summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void Deconstruct(out float x, out float y);
}