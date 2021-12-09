# rl-project-590: RL Training with PPO on clone of Flying Squirrel 

Required setup to view/run trained models:
1. Download Unity 2019.4.25f1
2. python 3.8.x (3.10.x does not work with pytorch!)
3. <code> pip install torch==1.7.0 -f https://download.pytorch.org/whl/torch_stable.html </code>
4. <code> pip install mlagents </code>

See agent playing NoWings with trained model using PPO:
1. add cloned folder as a project in Unityhub, then go to assets/scenes/noWings
2. go to Hierarchy>jank, make sure there is an .onnx file attached to jankgent in the inference model category
3. hit play

Play the game in Unity:
1. If havn't, add cloned folder as a project in Unityhub, then go to assets/scenes/noWings
2. go to Hierarchy>gameobjects, check that the jankgent component is on Heuristic Only
3. hit play
4. dive with the down arrow key

To train new models on the game: (example with testing.yaml in the ppo config folder)
<pre><code>mlagents-learn config/ppo/testing.yaml</code></pre>

See <code>mlagents-learn --help</code> for other possible flags.
