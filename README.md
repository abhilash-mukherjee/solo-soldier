<h1>Solo Soldier</h1>
    <h2>Controls</h2>
    <ul>
    <li>Move With Up-Down Arrow</li>
    <li>Rotate with mouse</li>
    <li>Shoot with left mouse click</li>
    <li>Throw grenade with right mouse click</li>

</ul>

<h3>Links</h3>
<ul>
    <li><strong>WebGL Build (ONLY WORKS ON PC): </strong><a href="https://abhilash-mukherjee.github.io/solo-soldier/">https://abhilash-mukherjee.github.io/solo-soldier/</a></li>
    <li><strong>Gameplay Video: </strong><a href="https://youtu.be/TI6v6vVSYlw">https://youtu.be/TI6v6vVSYlw</a></li>
</ul>


<h3>About</h3>
<p>Solosoldier is my 2nd full-fledged game using unity 3d. In this 3rd person shooter, you land in a city as a "solo-soldier" and enemies start attacking on you. Your task is to kill all of them and move to the next city.
</p>
<h3>Development Process</h3>
<h4>1. Player Movement:</h4>
<p> Player is moved forward-backward using keyboard input and rotated using mouse movement. 
</p>
<p>The movement animations are done usinganimation blend trees which ensure smooth transition between walking and running.
</p>

<h4>2. Firing:</h4>
<p>Raycasr is emitted from gun towards the transform.forward of player every time the player shoots. If the raycast hits an enemy, a "enemy-hit" event is raised which is listened by "EnemyHealth" script which handles damage on enemies.
</p>

<h4>3. Grenade Throwing: </h4>

<p>The grenade script shoots a projectile at the end of the "grenade_throw_animation" of the player. When grenade lands, a coroutine is started after which it explodes. The grenade has its area of influence, and if any object with health (other than player) is found inside the area, damage is dealt on that object.
</p>

<h4>4. Foot Enemy Movement:</h4>
<p>Enemies are nav mesh agents who target at player. They are activated once the player comes in close proximity of the enemy.
</p>
<h4>5. Helicopter Movement:</h4>

<p>The helicopter has AI that keeps following the player, and when the player is stationary, the helicopter comes downwards and start aiming at the enemy.
</p>
<h4>6. Tank Movement:</h4>

<p>Special script written to give realistic movement to tank. Tank moves only in straight lines, with it's head rotating 
</p>
<h4>7. Data Persistence: </h4>
<p>JSON serialization used to store data on local cache.
</p>
