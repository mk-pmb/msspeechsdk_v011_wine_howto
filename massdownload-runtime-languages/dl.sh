#!/bin/bash
# -*- coding: utf-8, tab-width: 2 -*-

function dl () {
  export LANG{,UAGE}=en_US.UTF-8  # make error messages search engine-friendly
  local SELFPATH="$(readlink -m -- "$BASH_SOURCE"/..)"
  cd -- "$SELFPATH" || return $?
  local CATEG="${1,,}"
  local LIST=()
  readarray -t LIST < <(grep -Fie "/MSSpeech_${CATEG^^}_" -- download-urls.txt)
  [ "${#LIST[@]}" -ge 1 ] || return 4$(echo E: >&2 \
    "Found no download URLs in category '$CATEG'. Try 'tts' or 'sr'.")
  local ITEM= FAILS=0
  mkdir --parents -- "$CATEG"
  for ITEM in "${LIST[@]}"; do
    cache-file-wget "$CATEG/" "$ITEM" && continue
    (( FAILS += 1 ))
    echo "$ITEM" >>download-fails.txt
  done
  [ "$FAILS" == 0 ] || return 4$(echo E: >&2 \
    "Had $FAILS fails. See download-fails.txt")
}


dl "$@"; exit $?
