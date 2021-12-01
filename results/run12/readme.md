Run12 saw a dramatic increase in cumulative rewards at the step around 300K.

It experiences a peak reward of ~72.


New reward system used in run12 compared to run9:

        if (velocity.x == 0){
            AddReward(-.05f);
        }
        if (velocity.x < 0){
            AddReward(-speed/10);
        }
        else if (velocity.x > controller.min_xforce + buffer){
            AddReward(speed/10);
        }

