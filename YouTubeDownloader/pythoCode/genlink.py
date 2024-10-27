import sys
from pytubefix import YouTube
try:
    if(len(sys.argv)<2):
        print("args must to set url video")
        sys.exit(1)
    filename = str(sys.argv[1])
    # יצירת אובייקט YouTube לסרטון
    yt = YouTube(filename)

    # בחירת הזרם באיכות הגבוהה ביותר
    stream = yt.streams.get_highest_resolution()

    # הדפסת ה-URL להורדה
    print(stream.url)
except Exception as ex:
    print(ex)
