# arduino-ddr
DDR Game on the Arduino Uno.

## Hardware
- Arduino Uno
- 4x Buttons
- 8x8 LCD
- Breadboard, jumper wires

## Dependencies
[Adafruit Neopixel](https://github.com/adafruit/adafruit_neopixel)

Plays tetris song by default, but you can create your own map/song by replacing the note array. You can run OsuManiaSongParser.cs to parse a downloaded piano beatmap from osu!mania, and paste it as the played note array.

### Note class details:
```c++
class Note {
public:
  uint32_t timestamp;
  uint16_t freq, holdTime;
  // the x should be 0,1,2, or 3, so that for 4 buttons their index matches up
  uint8_t x;

  constexpr Note(uint8_t x, uint32_t timestamp, uint16_t holdTime, uint16_t freq)
    : x(x), timestamp(timestamp), holdTime(holdTime), freq(freq) {}
};
```
