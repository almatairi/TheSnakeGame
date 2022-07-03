Project Description:


Our project is a reimplementation of the classic Snake game that increased in popularity in the 2000s with the game's version released on the Nokia device.


How to play:


The player controls a figure resembling a snake, which roams, picking up food and trying to avoid hitting its tail. Each time the snake eats a piece of food, its tail grows longer, making the game increasingly difficult. The snake can move freely on the screen, if it meets one of the edges, it will appear on the other side of the screen.
The snake is represented as a sequence of circles.


The food is also represented by circles, and is divided into three groups:

-Red circle: increases the score by one point, and the snake's body grows for one unit.

-Yellow circle: increases the score by two points, and the snake's body grows for two circles.

-Purple food: decreases the score by two points, and the snake's body becomes smaller for two circles.

The game will end when the head of the snake touches its body or the score becomes less than zero.

Project Implementation:

For the realization of this game, we need two classes:

-Circle class: is used to keep track of the X and Y values of each circle that will be drawn on the screen.

-Settings class: is used to specify default values for the height, width, and moving direction of each circle.

The initial state of the game:


  ![1](https://user-images.githubusercontent.com/101112086/177055034-7b30f5d6-92e0-4e27-879b-2cbc5c7a5ddf.png)


If the snake hits one of the walls it will reappear on the other sides.

![2](https://user-images.githubusercontent.com/101112086/177055073-fdb75382-b7d7-4b88-9f0a-c32f6fdc0ede.png)


KeyDown event: is used to make sure that the snake doesn’t turn to the opposite direction because it will go over its body (e.g.left to right).

KeyUp event:As soon as the keys are released the values of the keys will be reset to false so that we can check the next movement if it is correct.

Restart function: we keep all the default values we want to have before the game starts. The snake body will be reset to 6 elements. 


New instances of food will be generated in random positions-A new red food will reappear after the previous is “eaten”, a new yellow food will reappear every 5 seconds and a new purple food will reappear every 10 seconds.

Update picture box event:is the paint event linked to the picture box that will paint the elements accordingly.

Gametimer event: here we set the directions and make sure all the bodyparts are moving accordingly in the loop and appear on the other side and call the functions mentioned below.

EatFood():the score is incremented by one, and we add one unit to the body, and a next random red food is generated.

EatDoubleFood():the score is incremented by two, and we add two units to the body, and a next random yellow food is generated.

Shrink():the score is decremented by two, and we remove two units from the body, and a next random purple food is generated.

TakeSnapshot: if the take snapshot button is clicked then a bitmap with the height and width of the screen will be generated and will be converted to a jpg file with a message with the high score written.

  ![3](https://user-images.githubusercontent.com/101112086/177055110-6048d174-4875-49ed-ad13-fbfdc8c92eb3.jpg)



GameOver():the timer stops and we set the high score. The method is called when the head of the snake touches its body or the score becomes less than zero.

![4](https://user-images.githubusercontent.com/101112086/177055143-7584866c-bfb1-45d7-bc87-6b93f0e14368.png)

![5](https://user-images.githubusercontent.com/101112086/177055149-5f96e31a-1f88-4c4c-b2dd-ba5ce819532b.png)


 
