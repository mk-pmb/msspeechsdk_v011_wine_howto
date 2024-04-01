#!/bin/bash
# -*- coding: utf-8, tab-width: 2 -*-


function cs_make_test () {
  export LANG{,UAGE}=en_US.UTF-8  # make error messages search engine-friendly
  local INVO_DIR="$(dirname "$0")/"
  [ "$INVO_DIR" == ./ ] && INVO_DIR=
  local SELFPATH="$(readlink -m -- "$BASH_SOURCE"/..)"
  # cd -- "$SELFPATH" || return $?

  local PROG="$1"; shift
  case "$PROG" in
    --deps ) find_required_files "$@"; return $?;;
    -* ) echo E: "unsupported option: $PROG" >&2; return 2;;
  esac
  PROG="${PROG%.cs}"
  PROG="${PROG%.}"

  local TIMECAT=( cat )
  timecat --help &>/dev/null && TIMECAT=( timecat --timefmt '@%@%,' )

  local RQR_FILES=()
  readarray -t RQR_FILES < <(find_required_files "$PROG".cs)
  [ -n "${RQR_FILES[0]}" ] || return 3

  local DLL_NAMES=()
  readarray -t DLL_NAMES < <( (
    "$SELFPATH"/guess_required_dlls.sed -- "${RQR_FILES[@]}"
    ) | LANG=C sort -Vu)
  local DLL_REFS=()
  local DLL_NAME=
  for DLL_NAME in "${DLL_NAMES[@]}"; do
    for DLL_NAME in {,"$INVO_DIR","$SELFPATH"/}"$DLL_NAME"; do
      [ -f "$DLL_NAME" ] || continue
      DLL_REFS+=( /reference:"$DLL_NAME" )
      DLL_NAME=
      break
    done
    [ -z "$DLL_NAME" ] || echo "W: unable to find $DLL_NAME" >&2
  done

  local MCS_CMD=(
    mcs
    -out:"$PROG".exe
    "${DLL_REFS[@]}"
    "${RQR_FILES[@]}"
    )

  echo "D: compiler cmd: ${MCS_CMD[*]}"$'\n' >&2
  "${MCS_CMD[@]}" || return $?
  local UNBUF='stdbuf -i0 -o0 -e0'
  [ -f "$PROG".input.txt ] && exec <"$PROG".input.txt
  $UNBUF wine "$PROG".exe "$@" |& "${TIMECAT[@]}" | sed -ure '
    /^([0-9a-f]{3,8}:|)fixme:/d
    ' | $UNBUF tee -- "$PROG".log

  return 0
}


function find_required_files () {
  local TODO="$*"
  local CUR=
  local -A WANTED_BY=()
  for CUR in "$@"; do WANTED_BY["$CUR"]='.'; done
  local DEPS=()
  local DEP=
  local MISS_DEPS=0
  while [ -n "$TODO" ]; do
    # echo "D: $FUNCNAME: todo=[${TODO[*]}]" >&2
    CUR="${TODO%% *}"
    TODO="${TODO#* }"
    [ "$TODO" == "$CUR" ] && TODO=
    [ -n "$CUR" ] || continue
    DEPS=()
    readarray -t DEPS < <(
      sed -nre 's~^\s*//#require ~~p' -- "$CUR" | grep -oPe '\S+')
    for DEP in "${DEPS[@]}"; do
      [ -n "${WANTED_BY[$DEP]}" ] && continue
      if [ ! -f "$DEP" ]; then
        echo "W: missing dependency: $DEP for $CUR" >&2
        let MISS_DEPS="$MISS_DEPS+1"
        continue
      fi
      WANTED_BY["$DEP"]="$CUR"
      [ -n "$TODO" ] && TODO+=' '
      TODO+="$DEP"
    done
  done
  if [ "$MISS_DEPS" != 0 ]; then
    echo "E: $MISS_DEPS missing dependencies" >&2
    return 3
  fi
  printf '%s\n' "${!WANTED_BY[@]}" | LANG=C sort -Vu
}










cs_make_test "$@"; exit $?
