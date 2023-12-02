#!/bin/bash

if [ "$1" = "" ] ; then
  echo "Usage:  init-day <day>"
  exit 1
fi

DAY=$1

if [ "$DAY" -lt 10 ] ; then
  DAY="0$1"
fi

touch "./AdventOfCode/Inputs/$DAY.txt"
touch "./Tests/Solutions/$DAY.txt"

FILE_PATH="./AdventOfCode/Day$DAY.cs"
touch $FILE_PATH
printf "namespace AdventOfCode;

public sealed class Day%d : Day
{
    public override string Part1()
    {
        throw new NotImplementedException();
    }
}
" $DAY > $FILE_PATH
