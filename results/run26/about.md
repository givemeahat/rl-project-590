Run 26 -> 

Changes from run9:
Noticed that mlagent's discrete action defintion (0,1) with 0 mapped to diveFalse() and 1 mapped to dive() does not accurately reflect human playing experience - Heuristics with this action schema result in a need to spam the dive key to keep diving - completely different experience from "Getbuttondown" and "getbuttonup" used in the controller. The two value branch means that gravity is instantly switched after key press; doesn't mimic actual play experience -- during heuristic testing this also doesn't work as well since there seems to also be a lag between each action.

This new run now has one branch of discrete action with 3 values instead of 2:
        0. do nothing (retains gravity)
        1. dive added gravity; once added gravity in one click, unless select do nothing(0) immediately, gravity doesnt change 
        2. RESTORES original gravity 

This changed action schema resulted in way faster/higher cumulative reward compared to run9. At 5M steps reward peaked around ~200.

