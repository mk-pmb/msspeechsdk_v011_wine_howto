#!/bin/bash
# -*- coding: utf-8, tab-width: 2 -*-

function scan () {
  export LANG{,UAGE}=en_US.UTF-8  # make error messages search engine-friendly
  local SELFPATH="$(readlink -m -- "$BASH_SOURCE"/..)"
  cd -- "$SELFPATH" || return $?

  mkdir --parents cache
  local DL_URL='https://www.microsoft.com/en-us/download/details.aspx?id=27224'
  local ARCHIVED="https://web.archive.org/web/20240407/$DL_URL"
  cache-file-wget cache/ "$ARCHIVED" || return $?

  grep -Fe 'window.__DLCDetails__=' -- cache/details.aspx \
    | grep -oPe '"url":"[^"]+//download.microsoft.com/download/[^"]+\.msi' \
    | cut -d '"' -f 4- | sort -V >download-urls.txt
  wc --lines -- download-urls.txt
}


scan "$@"; exit $?
