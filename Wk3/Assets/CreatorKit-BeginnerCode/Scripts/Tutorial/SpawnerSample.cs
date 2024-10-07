using UnityEngine;
using CreatorKitCode;

public class SpawnerSample : MonoBehaviour
{
    public GameObject ObjectToSpawn;

   
    void Start()
    {
        // vars for potion while loop
        int potion_count = 1;
        int numofpotions = 4;
        
        Vector3 spawnPosition = transform.position;

        // Switch to a while loop for number of potions (i.e: while (numofpotions))
        // Allows for ease of viewing and easy to identify bugs
        
        LootAngle mylootangle = new LootAngle(45);

        while (potion_count <= numofpotions){
            SpawnPotion(mylootangle.next_angle());
            // mylootangle.angle = mylootangle.angle + mylootangle.step;
            potion_count += 1;
        }
    }
    
    public class LootAngle{
        // Legend : angle = current angle , step = increments of angle
        public int angle ;
        public int step ;

        public LootAngle(int increment)
        {
            step = increment;
            angle = 0;
        }

        public int next_angle (){
            int current_angle = angle;
            angle = Helpers.WrapAngle(angle+step);
            return current_angle;
        }
    }

    void SpawnPotion(int angle){
        int radius = 5;
        
        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.right;
        Vector3 spawnPosition = transform.position + direction * radius;
        Instantiate(ObjectToSpawn, spawnPosition, Quaternion.identity);
    }
}

