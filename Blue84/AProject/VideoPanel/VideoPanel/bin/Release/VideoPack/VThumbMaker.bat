start

ffmpeg.exe -i %1 -ss %2 -vframes 1 -r 1 -ac 1 -ab 2 -s 120*100 -f  image2 %3  

exit
 
