#!/bin/bash
# -*- coding: utf-8, tab-width: 2 -*-


function cs_make_test () {
  export LANG{,UAGE}=en_US.UTF-8  # make error messages search engine-friendly
  local SELFPATH="$(readlink -m "$BASH_SOURCE"/..)"
  # cd "$SELFPATH" || return $?

  local PROG="$1"; shift
  PROG="${PROG%.cs}"

  local TIMECAT=( cat )
  timecat --help &>/dev/null && TIMECAT=( timecat --timefmt '@%@%,' )

  mcs /reference:Microsoft.Speech.dll "$PROG".cs || return $?
  wine "$PROG".exe "$@" |& "${TIMECAT[@]}" | sed -re '
    /^([0-9a-f]{3,8}:|)fixme:/d
    ' | tee -- "$PROG".log

  return 0
}










cs_make_test "$@"; exit $?