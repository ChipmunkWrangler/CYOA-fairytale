CONST FEMALE = 0
CONST MALE = 1
VAR NAME = "Talea"
VAR SEX = FEMALE
VAR CUR_EPISODE = 0
CONST NUM_EPISODES = 7
VAR FRIEND0 = false
VAR FRIEND1 = false
VAR FRIEND2 = false

-> setup -> intro -> body -> resolution

=== setup
Are you a boy or a girl?
 * [I'm a boy]
   ~ SEX = MALE
 * [I'm a girl]
//- <>, of course!
- What is your name? #COMMAND #RequestInput #NAME #Talea
->->
 
 === intro
<b><size=50>{NAME}'s Night Adventure</size></b>
{NAME} wakes up. {He()} isn't at home. {He()} isn't in {his()} bed. {He()} is in a midnight forest. 
 * [Should {he()} call for {his()} parents?] 
   "Dad?" 
   Silence. 
   "Mom?"
   Darkness.
   {NAME} is all alone. 
   Or maybe not! <>
 * [Or look around?] 
  {NAME} peers into the darkness, turning slowly. 
- ->->

=== body
 ~ CUR_EPISODE++
 {CUR_EPISODE <= NUM_EPISODES: -> pickEpisode -> body}
 ->->
 
=== pickEpisode
  {~ ->light | -> sound}

=== light
Over there -- a light!
 * It's a fairy!
   -> fairy
 * It's a campfire.
   {shuffle:
 //  - {not fairy.trollInterrupt: ->trolls} 
   - -> trolls
   - -> bandits
   //- {not fairy.banditInterrupt: ->bandits} 
   - ->gypsies
   }

=== sound
A branch cracks! What's that behind {NAME}?
 * A wolf!
   -> wolf
 * A giant!
   -> giant

=== trolls
 Trolls
 ->->
=== bandits
  Bandits
 ->->
=== gypsies
  Gypsies
 ->->
=== wolf
  Wolf
  ->->
=== giant
  Giant
  ->->
  
=== fairy
  <> She floats towards {NAME}.
  "Beautiful child, lost in the wild! Are you alone, far from home? Do not shiver in the night -- come to the river! The moon shines bright on our fairy ring. Come dance, come sing!"
  * DANCE
   DANCE IN THE FAIRY RING....
   {
    - hasFriend():
        AND BE RESCUED
    - CUR_EPISODE < NUM_EPISODES:
        {~ -> trollInterrupt | -> banditInterrupt }
    - else:
        DANCE FOREVER
    }
  * RUN
- ->->
= trollInterrupt
TROLL INTERRUPT
->->
= banditInterrupt
BANDIT INTERRUPT
->->
=== resolution
THE END
-> END

=== function hasFriend 
~ return FRIEND0 || FRIEND1 || FRIEND2

=== function He ===
{ SEX == FEMALE: She|He}
   
=== function he ===
{ SEX == FEMALE: she|he}

=== function his
{ SEX == FEMALE: her|his}
