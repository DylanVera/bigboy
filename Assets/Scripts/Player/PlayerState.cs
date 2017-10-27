using UnityEngine;
using System.Collections;


/// <summary>
/// The various states you can use to check if your character is doing something at the current frame
/// by Renaud Forestié
/// </summary>

public class CharacterBehaviorState
{
    public bool CanJump { get; set; }
    public bool CanShoot { get; set; }
    public bool CanMelee { get; set; }
    public bool CanMoveFreely { get; set; }
    public int NumberOfJumpsLeft;
    public bool Dashing { get; set; }
    public bool Running { get; set; }
    public bool WallClinging { get; set; }
    public bool Jetpacking { get; set; }
    public bool Diving { get; set; }
    public bool Firing { get; set; }
    public bool FiringStop { get; set; }
    public bool MeleeAttacking { get; set; }
    public bool LadderColliding { get; set; }
    public bool Gripping { get; set; }
    public bool Frozen { get; set; }
    public bool Dangling { get; set; }
    public bool LadderTopColliding { get; set; }
    public bool LadderClimbing { get; set; }
    public float LadderClimbingSpeed { get; set; }
    public int FiringDirection { get; set; }
    public bool IsDead { get; set; }
    public bool JustStartedJumping;
    public bool Jumping;
    public bool DoubleJumping;



    /// <summary>
    /// Initializes all states to their default value
    /// </summary>
    public virtual void Initialize()
    {
        CanMoveFreely = true;
        CanShoot = true;
        CanMelee = true;
        Dashing = false;
        Running = false;
        WallClinging = false;
        Jetpacking = false;
        Diving = false;
        LadderClimbing = false;
        LadderColliding = false;
        LadderTopColliding = false;
        LadderClimbingSpeed = 0f;
        Firing = false;
        FiringStop = false;
        FiringDirection = 3;
        MeleeAttacking = false;
        Frozen = false;
    }

    public virtual void UpdateReset()
    {
        CanShoot = true;
        JustStartedJumping = false;

        if (!Firing)
        {
            FiringStop = false;
        }
    }

	public bool IsCollidingRight { get; set; }
    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingAbove { get; set; }
    public bool IsCollidingBelow { get; set; }
    public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; } }
    public float LateralSlopeAngle { get; set; }
    public float BelowSlopeAngle { get; set; }
    public bool SlopeAngleOK { get; set; }
    public bool OnAMovingPlatform { get; set; }
    public bool IsGrounded { get { return IsCollidingBelow; } }
    public bool IsFalling { get; set; }
    public bool WasGroundedLastFrame { get; set; }
    public bool WasTouchingTheCeilingLastFrame { get; set; }
    public bool JustGotGrounded { get; set; }

    public virtual void Reset()
    {
        IsCollidingLeft =
        IsCollidingRight =
        IsCollidingAbove =
        SlopeAngleOK =
        JustGotGrounded = false;
        IsFalling = true;
        LateralSlopeAngle = 0;
    }

    public override string ToString()
    {
        return string.Format("(controller: r:{0} l:{1} a:{2} b:{3} down-slope:{4} up-slope:{5} angle: {6}",
        IsCollidingRight,
        IsCollidingLeft,
        IsCollidingAbove,
        IsCollidingBelow,
        LateralSlopeAngle);
    }
}

