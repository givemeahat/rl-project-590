This is the first sorta bug-free runs.

In the middle of running however, I realized that there is a flaw with the reward system; 

The original reward system was like this: 

        if (velocity.x < controller.min_xforce + buffer){
            AddReward(-speed/10);
        }
        else if (velocity.x > controller.min_xforce + buffer){
            AddReward(speed/10);
        }

Essentially, the first reward system was designed to heavily penalize any speed less than the min static x velocity (min_xforce); The buffer is added to increase the threshold of reward since due to the physics system there is some small inherent acceleration when going downhill even if the agent is doing nothing.

The fatal flaw in this design is its inability to reflect a desired range of reward for the velocity.x range (0, controller.min_xforce + buffer).

If velocity.x is 0 (when agent just holds down the dive key forever), this reward schema would incur 0 loss, but incur some sort of increasing loss when going either way; Schema therefore encourages diving forever incuring 0 loss.

This flaw is reflected in the tensorboard analysis;


New reward system used in run12:

        if (velocity.x == 0){
            AddReward(-.05f);
        }
        if (velocity.x < 0){
            AddReward(-speed/10);
        }
        else if (velocity.x > controller.min_xforce + buffer){
            AddReward(speed/10);
        }
        
<p align="center" width="100%">
  <img width="40%" src="https://github.com/iigindesign/rl-project-590/blob/Dev/images/run26.png">
</p>
