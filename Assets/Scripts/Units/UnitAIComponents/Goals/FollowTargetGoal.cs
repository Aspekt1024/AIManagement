using Aspekt.AI;
using TestUnitLabels;

public class FollowTargetGoal : AIGoal
{
    protected override void SetConditions()
    {
        AddCondition(AILabels.TargetReached.ToString(), true);
    }

}