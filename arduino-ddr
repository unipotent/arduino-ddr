#include <Adafruit_NeoPixel.h>
#include <Adafruit_NeoMatrix.h>

#define NEO_PIN 7
#define DIMENSIONS 8

// define notes, stolen from arduin-songs library
#define NOTE_B0 31
#define NOTE_C1 33
#define NOTE_CS1 35
#define NOTE_D1 37
#define NOTE_DS1 39
#define NOTE_E1 41
#define NOTE_F1 44
#define NOTE_FS1 46
#define NOTE_G1 49
#define NOTE_GS1 52
#define NOTE_A1 55
#define NOTE_AS1 58
#define NOTE_B1 62
#define NOTE_C2 65
#define NOTE_CS2 69
#define NOTE_D2 73
#define NOTE_DS2 78
#define NOTE_E2 82
#define NOTE_F2 87
#define NOTE_FS2 93
#define NOTE_G2 98
#define NOTE_GS2 104
#define NOTE_A2 110
#define NOTE_AS2 117
#define NOTE_B2 123
#define NOTE_C3 131
#define NOTE_CS3 139
#define NOTE_D3 147
#define NOTE_DS3 156
#define NOTE_E3 165
#define NOTE_F3 175
#define NOTE_FS3 185
#define NOTE_G3 196
#define NOTE_GS3 208
#define NOTE_A3 220
#define NOTE_AS3 233
#define NOTE_B3 247
#define NOTE_C4 262
#define NOTE_CS4 277
#define NOTE_D4 294
#define NOTE_DS4 311
#define NOTE_E4 330
#define NOTE_F4 349
#define NOTE_FS4 370
#define NOTE_G4 392
#define NOTE_GS4 415
#define NOTE_A4 440
#define NOTE_AS4 466
#define NOTE_B4 494
#define NOTE_C5 523
#define NOTE_CS5 554
#define NOTE_D5 587
#define NOTE_DS5 622
#define NOTE_E5 659
#define NOTE_F5 698
#define NOTE_FS5 740
#define NOTE_G5 784
#define NOTE_GS5 831
#define NOTE_A5 880
#define NOTE_AS5 932
#define NOTE_B5 988
#define NOTE_C6 1047
#define NOTE_CS6 1109
#define NOTE_D6 1175
#define NOTE_DS6 1245
#define NOTE_E6 1319
#define NOTE_F6 1397
#define NOTE_FS6 1480
#define NOTE_G6 1568
#define NOTE_GS6 1661
#define NOTE_A6 1760
#define NOTE_AS6 1865
#define NOTE_B6 1976
#define NOTE_C7 2093
#define NOTE_CS7 2217
#define NOTE_D7 2349
#define NOTE_DS7 2489
#define NOTE_E7 2637
#define NOTE_F7 2794
#define NOTE_FS7 2960
#define NOTE_G7 3136
#define NOTE_GS7 3322
#define NOTE_A7 3520
#define NOTE_AS7 3729
#define NOTE_B7 3951
#define NOTE_C8 4186
#define NOTE_CS8 4435
#define NOTE_D8 4699
#define NOTE_DS8 4978
#define REST 0

Adafruit_NeoMatrix pixels = Adafruit_NeoMatrix(DIMENSIONS, DIMENSIONS, NEO_PIN,
                                               NEO_MATRIX_BOTTOM + NEO_MATRIX_RIGHT + NEO_MATRIX_ROWS + NEO_MATRIX_PROGRESSIVE,
                                               NEO_GRB + NEO_KHZ800);

class Note {
public:
  uint32_t timestamp;
  uint16_t freq, holdTime;
  // the x should be 0,1,2, or 3, so that for 4 buttons their index matches up
  uint8_t x;

  // the constexpr is so it calculates this on compile time instead of on runtime or else it will be dynamic initialization and cant be
  // used in progmem in the note array
  constexpr Note(uint8_t x, uint32_t timestamp, uint16_t holdTime, uint16_t freq)
    : x(x), timestamp(timestamp), holdTime(holdTime), freq(freq) {}
};

// this is a class that makes tracking buttons easier
class Button {
private:
  uint8_t pin;                              // the pin the button is on
  bool pressedThisFrame, pressedLastFrame;  // track state
public:
  Button(uint8_t pin)
    : pin(pin) {}
  void setup() {
    pinMode(pin, INPUT_PULLUP);  // set to input pullup at pin
  }
  void process()  // record state
  {
    pressedLastFrame = pressedThisFrame;
    pressedThisFrame = digitalRead(pin) == LOW;
  }
  bool checkHold() {
    return pressedThisFrame;
  }
  bool checkRelease() {
    return pressedLastFrame && !pressedThisFrame;  // clicked last frame and released
  }
  bool checkPressed() {
    return !pressedLastFrame && pressedThisFrame;  // just clicked this frame
  }
};
// x offset from side
//screen height
// judgement line is where you have to hit it
const uint8_t X_OFFSET = 2, SCREEN_Y = 8, JUDGEMENT_LINE_Y = 1, SPEAKER_PIN = 8;
// the scrollspeed, higher = slower
// the note time is how long each tone will be played for
// target frame time of 60fps so the time between each frame in ms is 1000/60
const uint16_t SCROLL_SPEED = 125, NOTE_TIME = 208, TARGET_FRAME_TIME = 1000 / 60;
const int16_t FORGIVENESS = 400;  //the forgivable amount of time away from the proper time your supposed to hit a note

// notes to be played
// tetris song stolen from : https://github.com/robsoncouto/arduino-songs/blob/master/tetris/tetris.ino
const Note NOTES[] PROGMEM = {
  // --- Section 1 ---
  Note(0, 3000, 416, NOTE_E5),
  Note(1, 3416, 0, NOTE_B4),
  Note(2, 3624, 0, NOTE_C5),
  Note(1, 3832, 416, NOTE_D5),
  Note(0, 4248, 0, NOTE_C5),
  Note(1, 4456, 0, NOTE_B4),
  Note(0, 4664, 416, NOTE_A4),
  Note(3, 5080, 0, NOTE_A4),
  Note(2, 5288, 0, NOTE_C5),
  Note(3, 5496, 416, NOTE_E5),
  Note(0, 5912, 0, NOTE_D5),
  Note(1, 6120, 0, NOTE_C5),
  Note(2, 6328, 624, NOTE_B4),
  Note(3, 6952, 0, NOTE_C5),
  Note(2, 7160, 416, NOTE_D5),
  Note(1, 7576, 416, NOTE_E5),
  Note(1, 7992, 416, NOTE_C5),
  Note(2, 8408, 416, NOTE_A4),
  Note(1, 8824, 416, NOTE_A4),
  // REST, 4 advances timestamp by 416 (8824 + 416 + 416 = 9656)

  // --- Section 2 ---
  // REST, 8 advances timestamp by 208 (9656 + 208 = 9864)
  Note(3, 9864, 416, NOTE_D5),
  Note(0, 10280, 0, NOTE_F5),
  Note(2, 10488, 416, NOTE_A5),
  Note(3, 10904, 0, NOTE_G5),
  Note(2, 11112, 0, NOTE_F5),
  Note(3, 11320, 624, NOTE_E5),
  Note(0, 11944, 0, NOTE_C5),
  Note(1, 12152, 416, NOTE_E5),
  Note(2, 12568, 0, NOTE_D5),
  Note(0, 12776, 0, NOTE_C5),
  Note(2, 12984, 416, NOTE_B4),
  Note(1, 13400, 0, NOTE_B4),
  Note(3, 13608, 0, NOTE_C5),
  Note(3, 13816, 416, NOTE_D5),
  Note(2, 14232, 416, NOTE_E5),
  Note(1, 14648, 416, NOTE_C5),
  Note(0, 15064, 416, NOTE_A4),
  Note(0, 15480, 416, NOTE_A4),
  // REST, 4 advances timestamp by 416 (15480 + 416 + 416 = 16312)

  // --- Section 3 ---
  Note(3, 16312, 832, NOTE_E5),
  Note(2, 17144, 832, NOTE_C5),
  Note(1, 17976, 832, NOTE_D5),
  Note(0, 18808, 832, NOTE_B4),
  Note(1, 19640, 832, NOTE_C5),
  Note(2, 20472, 832, NOTE_A4),
  Note(3, 21304, 1664, NOTE_B4),

  // --- Section 4 ---
  Note(0, 22968, 832, NOTE_E5),
  Note(1, 23800, 832, NOTE_C5),
  Note(2, 24632, 832, NOTE_D5),
  Note(3, 25464, 832, NOTE_B4),
  Note(2, 26296, 416, NOTE_C5),
  Note(1, 26712, 416, NOTE_E5),
  Note(0, 27128, 832, NOTE_A5),
  Note(3, 27960, 1664, NOTE_GS5),

  // --- Section 5 (Repeat of Section 1) ---
  Note(3, 29624, 416, NOTE_E5),
  Note(2, 30040, 0, NOTE_B4),
  Note(1, 30248, 0, NOTE_C5),
  Note(2, 30456, 416, NOTE_D5),
  Note(3, 30872, 0, NOTE_C5),
  Note(2, 31080, 0, NOTE_B4),
  Note(3, 31288, 416, NOTE_A4),
  Note(0, 31704, 0, NOTE_A4),
  Note(1, 31912, 0, NOTE_C5),
  Note(0, 32120, 416, NOTE_E5),
  Note(3, 32536, 0, NOTE_D5),
  Note(2, 32744, 0, NOTE_C5),
  Note(1, 32952, 624, NOTE_B4),
  Note(0, 33576, 0, NOTE_C5),
  Note(1, 33784, 416, NOTE_D5),
  Note(2, 34200, 416, NOTE_E5),
  Note(2, 34616, 416, NOTE_C5),
  Note(1, 35032, 416, NOTE_A4),
  Note(2, 35448, 416, NOTE_A4),
  // REST, 4 advances timestamp by 416 (35448 + 416 + 416 = 36280)

  // --- Section 6 ---
  // REST, 8 advances timestamp by 208 (36280 + 208 = 36488)
  Note(2, 36488, 416, NOTE_D5),
  Note(1, 36904, 0, NOTE_F5),
  Note(3, 37112, 416, NOTE_A5),
  Note(0, 37528, 0, NOTE_G5),
  Note(1, 37736, 0, NOTE_F5),
  // REST, 8 advances timestamp by 208 (37736 + 208 + 208 = 38152)
  Note(1, 38152, 416, NOTE_E5),
  Note(2, 38568, 0, NOTE_C5),
  Note(0, 38776, 416, NOTE_E5),
  Note(3, 39192, 0, NOTE_D5),
  Note(2, 39400, 0, NOTE_C5),
  // REST, 8 advances timestamp by 208 (39400 + 208 + 208 = 39816)
  Note(3, 39816, 416, NOTE_B4),
  Note(2, 40232, 0, NOTE_C5),
  Note(1, 40440, 416, NOTE_D5),
  Note(0, 40856, 416, NOTE_E5),
  // REST, 8 advances timestamp by 208 (40856 + 416 + 208 = 41480)
  Note(3, 41480, 416, NOTE_C5),
  Note(0, 41896, 0, NOTE_A4),
  Note(3, 42104, 416, NOTE_A4)
  // Final REST, 4 is ignored as it marks the end of the song
};
const uint16_t SIZE = sizeof(NOTES) / sizeof(NOTES[0]);  // array size
unsigned long startTime;
// windowStartIndex which will start at 0 and grow as the old notes go out of range
// score will be number of notes hit, missed will be number of notes missed
uint16_t windowStartIndex, score, missed;
int8_t textXCoord = DIMENSIONS;  // start of text should be at the edge of the screen

// create buttons on pins
Button buttons[] = {
  Button(5),
  Button(4),
  Button(3),
  Button(2),
};

void setup() {
  Serial.begin(9600);
  pixels.begin();
  pixels.setBrightness(25);   // make it less bright
  pixels.setTextWrap(false);  // Allows text to slide offscreen horizontally for displaying score

  for (Button &button : buttons) // set up buttons
    button.setup();

  // record start time
  startTime = millis();
}
void loop() {
  pixels.fillScreen(0);          // clear
  if (windowStartIndex >= SIZE)  // out of range, meaning done, so show score infinitely
  {
    String scrollText = "Score: " + String(score) + " Missed: " + String(missed);
    pixels.setCursor(textXCoord, 0);  // position at the x
    pixels.print(scrollText);         // write the text

    // move cursor left by 1 pixel for the next frame
    textXCoord--;
    // calculate total message width in pixels 6 pixels per character
    int leftBoundary = scrollText.length() * 6;

    // reset when the entire message passes past the left boundary
    if (textXCoord < -leftBoundary) {
      textXCoord = DIMENSIONS;  // Reset position to enter from the right side again
    }
    pixels.show();  // show
    delay(100);     // delay to not be too fast
    return;
  }

  unsigned long timeSinceStart = millis() - startTime;
  //update each button to check if pressed this frame, and also set the judgement line color
  for (int i = 0; i < 4; i++) {
    buttons[i].process();
    pixels.setPixelColor(JUDGEMENT_LINE_Y * 8 + i + X_OFFSET, 255, 255, 255);
  }

  for (int i = windowStartIndex; i < SIZE; i++) {
    Note note = Note(0, 0, 0, 0);              // create a note to copy the item to
    memcpy_P(&note, &NOTES[i], sizeof(Note));  // copy the note at this index from the flash memory to the local note variable

    // time until hit in ms
    long timeUntilHit = note.timestamp - timeSinceStart;

    int yPos = getYPos(timeUntilHit);
    // out of range, which means everything after in the loop will be out of range too, so break
    // also it needs to be casted to an integer or else yPos will be casted to be unsigned, and so if its negative when its unsigned it turns into a huge number instead and makes this evaluate true when it shouldnt
    if (yPos > (int)SCREEN_Y) {
      break;
    }
    // not hold note
    if (note.holdTime == 0)  
    {
       // didnt hit it in time but its gone now
      if (yPos < 0) {
        windowStartIndex++;
        recordMiss();
      } else {
        // check if need hit and if clicked
        if (hit(timeUntilHit) && buttons[note.x].checkPressed()) {
          // if clicked play the note
          tone(note.freq);
          windowStartIndex++;  // remove this note from the window because its hit so its done
          recordHit();
        }
        // write at position
        writeNote(note.x, yPos);
      }
    } else { // is hold note
      long timeUntilHoldEnd = timeUntilHit + note.holdTime;
      int yHoldEnd = getYPos(timeUntilHoldEnd);
      yPos = max(0, yPos);                 // make sure smaller than screen
      yHoldEnd = min(yHoldEnd, SCREEN_Y);  // make sure smaller than screen

      //  if roughly hit first part of the hold note, or if roughly hit the end of the note
      if ((hit(timeUntilHit) && buttons[note.x].checkPressed()) || (hit(timeUntilHoldEnd) && buttons[note.x].checkRelease())) {
        // its being held, so set the start index of the pixels to be written to later to the judgement line and not fall below
        yPos = JUDGEMENT_LINE_Y;
        // play tone
        tone(note.freq);
        recordHit();

        // if it was the the end that was hit, it means it should now be removed from the window
        if (hit(timeUntilHoldEnd)) {
          windowStartIndex++;
        }
      }
      // if you are definitely supposed to be hitting the note
      else if (JUDGEMENT_LINE_Y >= yPos && JUDGEMENT_LINE_Y <= yHoldEnd) {
        if (buttons[note.x].checkHold()) {
          // its being held, so set the start index of the pixels to be written to later to the judgement line and not fall below
          yPos = JUDGEMENT_LINE_Y;
          // play tone
          tone(note.freq);
          recordHit();
        } else {
          recordMiss();
        }
      } else if (JUDGEMENT_LINE_Y > yHoldEnd) {
        windowStartIndex++;  // the note is outside the window so remove it
      }
      for (int i = yPos; i <= yHoldEnd; i++) {
        // write from the start yPos to the end y at the note.x
        writeNote(note.x, i);
      }
    }
  }

  // write judgement line checking if held
  for (int i = 0; i < 4; i++) {
    if (buttons[i].checkHold()) { // if the button there is pressed show a different color to show pressed
      pixels.setPixelColor(JUDGEMENT_LINE_Y * 8 + i + X_OFFSET, pixels.Color(0, 255, 0));
    }
  }
  pixels.show();

  // keep a consistent frame rate so that it doesn't call recordHit or recordMiss depending on how fast the loop runs
  unsigned long timeSinceFrameStart = millis() - timeSinceStart + startTime;
  if (timeSinceFrameStart < TARGET_FRAME_TIME) {// if the time used was less than the target frame time, just delay until it is
    delay(TARGET_FRAME_TIME - timeSinceFrameStart);
  }
}
void writeNote(uint8_t x, uint8_t y) {
  // since its like each row and column is 8, you multiply y by the rowCount, and then add the x
  uint8_t xy = y * 8 + x + X_OFFSET;
  // write position
  pixels.setPixelColor(xy, getNoteColor(x));  // set the pixel color at the position
}
uint32_t getNoteColor(uint8_t x) {
  if (x % 2 == 0) { // if even
    return pixels.Color(255, 0, 0);
  }
  else { // if odd
    return pixels.Color(0, 0, 255);
  }
  // so that every other note column is a different color
}
bool hit(long timeUntilHit)  // gets if it should be hit but taking the forgiveness value into consideration
{
  Serial.print(F("Time until hit "));
  Serial.println(timeUntilHit);
  return timeUntilHit <= FORGIVENESS && timeUntilHit >= -FORGIVENESS;
}
// calculates the y position of a note given the time until it needs to be hit
long getYPos(long timeUntilHit) {
  return JUDGEMENT_LINE_Y + timeUntilHit / SCROLL_SPEED;  // divide by scrollspeed to not make it too fast
}
// tone function to make calling tone easier
void tone(uint16_t freq) {
  tone(SPEAKER_PIN, freq, NOTE_TIME);
}
void recordMiss() {
  Serial.println(F("Missed"));
  missed++;
}
void recordHit() {
  Serial.println(F("Hit"));
  score++;
}




