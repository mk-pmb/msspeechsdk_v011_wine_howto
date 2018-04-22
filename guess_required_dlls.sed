#!/bin/sed -nurf
# -*- coding: UTF-8, tab-width: 2 -*-

s~\r~~g
s~^\s*using ((Microsoft|System)\.Speech\.)\S+;$~\1dll~p
