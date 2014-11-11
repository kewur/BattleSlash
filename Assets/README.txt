

	Maze Generation:
	
		using the depth first search backtracking algorithm, the first thing I did was to choose a side of the maze (top, left, right, bottom) for the exit
		and randomly get one of the blocks on that side to set it as "Exit". Corners are excluded from this. Once I choose the Exit, I start from there to build the maze.
		The maze is initially all walls. The outer layer is always excluded from the maze building function so the Maze is 2*n+1 X 2*n+1 size. From the Exit point (which is our 
		start point), by going in a random direction of available neighbours by 2 blocks (available neighbour = it's not out of bounds of maze or is not a road already), the selected 
		direction's walls are made paths. if ever a path is stuck and has no available neighbours. by using backtracking the first cell that has available neighbours is selected and continued from there
		until there are no unvisited cells remaining in the Maze. The reason 2 walls are being changed to paths at a single increment is because the Maze does not have walls like in a typical maze, and by incrementing it in 2s 
		gives the maze room for the walls. 

		initial size of the maze is even (this is forced in the GameManager object at the start). The size of the Maze is adjustable if these values are set to different
		values and the maze generator will generate a different maze each time.

		problems: At first, because the DFS can be implemented as a graph search given a Stack, I tried to combine the search algorithm for the Monster's AI and reuse the code in there. But due to some additional functionallity needed
		in the Maze generation process, I decided to separate those functions. 
		
		total time: 2-3 hours. including my attempts at combining graph search and maze generation which took 1 hour until I decided to go with another approach.
		
	Painting player visited cells to white:
	
		I'm holding a bool[,] object the same size as the grid, and cast a ray from the player's position downwards. When it hits a block, gets the name of the block (block names are named as to correspond to their index values in the Grid[,] object during generation), looks up
		if the grid was visited before, if not paints it white.
		
		total time: 15-30 minutes.
		
	
	Dynamic batching:
	
		Because I wanted to use flash light and other lights, I wanted to reduce draw calls. In order to do that I have changed the random material changing function and support sharing materials between blocks. 
		total time: 30 minutes.
	
	Monster's AI:
	
		The monster's path-finding uses the A* search algorithm with a container that implements the IGenericList interface (PriorityQueue in this case). This allows me to use different containers for different kinds of search algorithms with ease.
		
		problems: The Monster would get stuck whenever I restarted the level. That was due to the fact that I was not resetting the values properly on the restart. 

		total implementation time: 3 hours.
		
	Maze walls to one colour:
		I have implemented the maze walls as one colour but it was very hard to discern the corners of the corridors, so their colours are assigned from a set of materials that has slightly different colours.

Assets used: I have used the Moster2Pack asset for my Monster's model. It's a free asset and is already included in the project.	


I hope you like it! 