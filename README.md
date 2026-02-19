Hi Professor and Andrew,

This was a cool miner! First things first, I adjusted the thirst level from 15 to 12 to create a performance margin in the system. 
The previous code had the miner change to a Drinking state only when its thirst level reached 15, i.e., at maximum operational level. 
This code had the miner operate on the point of failure (condition), thereby resulting in an inefficient operation.
The change to a thirst level of 12 provides a three unit safety margin. 
The miner now operates in a more efficient manner because it transitions proactively rather than reactively. 
The state machine code remains unchanged. The change here is in the timing.
The change to a thirst level of 12 is a deliberate one. A threshold of 10 would be overly conservative and reduce mining productivity by triggering transitions too early. 
A threshold of 14 wouldn't be marginally different from 15 meaning it wouldn't really alter the miner's behavior. Twelve provides a balance between output and stability (difference of 3). 
The miner now operates within a certain level of performance, thereby resulting in a stable and efficient state machine operation.
