thisdir := mcs
SUBDIRS := 
include ../build/rules.make

EXTRA_DISTFILES = \
	mcs.csproj		\
	compiler.doc		\
	mcs.sln		\
	cs-parser.jay		\
	mcs.exe.sources		\
	NOTES			\
	TODO			\
	ikvm.cs

ifeq (basic, $(PROFILE))
PROGRAM = $(topdir)/class/lib/basic/mcs.exe
else
PROGRAM = $(topdir)/class/lib/build/mcs.exe
LOCAL_MCS_FLAGS += -lib:$(topdir)/class/lib/build
endif

LOCAL_MCS_FLAGS += -d:STATIC

PROGRAM_INSTALL_DIR = $(mono_libdir)/mono/4.0

PROGRAM_COMPILE = $(BOOT_COMPILE)

BUILT_SOURCES = cs-parser.cs

CLEAN_FILES += y.output

# Use -cvt for parser debug version
%-parser.cs: %-parser.jay $(topdir)/jay/skeleton.cs
	$(topdir)/jay/jay -cv < $(topdir)/jay/skeleton.cs $< > jay-tmp.out && mv jay-tmp.out $@


KEEP_OUTPUT_FILE_COPY = yes

include ../build/executable.make

csproj-local:
	config_file=`basename $(PROGRAM) .exe`-$(PROFILE).input; \
	echo $(thisdir):$$config_file >> $(topdir)/../mono/msvc/scripts/order; \
	(echo $(is_boot); \
	echo $(BOOTSTRAP_MCS);	\
	echo $(USE_MCS_FLAGS) $(LIBRARY_FLAGS) $(LIB_MCS_FLAGS); \
	echo $(PROGRAM); \
	echo $(BUILT_SOURCES); \
	echo $(PROGRAM); \
	echo $(response)) > $(topdir)/../mono/msvc/scripts/inputs/$$config_file
	
ifeq (basic, $(PROFILE))
install-local:
	$(MKINSTALLDIRS) $(DESTDIR)$(PROGRAM_INSTALL_DIR)
	$(INSTALL_BIN) $(PROGRAM) $(DESTDIR)$(PROGRAM_INSTALL_DIR)
	test ! -f $(PROGRAM).mdb || $(INSTALL_BIN) $(PROGRAM).mdb $(DESTDIR)$(PROGRAM_INSTALL_DIR)
endif

#
# Below this line we have local targets used for testing and development
#

# Testing targets

TIME = time

# This used to be called test, but that conflicts with the global
# recursive target.

btest: mcs2.exe mcs3.exe
	ls -l mcs2.exe mcs3.exe

mcs2.exe: $(PROGRAM)
	$(TIME) $(RUNTIME) $(RUNTIME_FLAGS) $(PROGRAM) $(USE_MCS_FLAGS) -target:exe -out:$@ $(BUILT_SOURCES) @$(response)

mcs3.exe: mcs2.exe
	$(TIME) $(RUNTIME) $(RUNTIME_FLAGS) ./mcs2.exe $(USE_MCS_FLAGS) -target:exe -out:$@ $(BUILT_SOURCES) @$(response)

wc:
	wc -l $(BUILT_SOURCES) `cat $(sourcefile)`

ctest: 
	rm -f mcs2.exe mcs3.exe
	$(MAKE) USE_MCS_FLAGS="-d:NET_1_1 -d:ONLY_1_1" btest

# we need this because bash tries to use its own crappy timer
FRIENDLY_TIME = $(shell which time) -f'%U seconds'

do-time : $(PROGRAM)
	@ echo -n "Run 1:   "
	@ rm -f mcs2.exe
	@ $(MAKE) TIME="$(FRIENDLY_TIME)" mcs2.exe > /dev/null || (echo FAILED; exit 1)
	@ echo -n "Run 2:   "
	@ rm -f mcs3.exe
	@ $(MAKE) TIME="$(FRIENDLY_TIME)" mcs3.exe > /dev/null || (echo FAILED; exit 1)
	@ $(MAKE) do-corlib

do-corlib:
	@ echo -n "corlib:  "
	@ rm -f ../class/lib/mscorlib.dll
	@ cd ../class/corlib ; $(MAKE) BOOTSTRAP_MCS='$(FRIENDLY_TIME) mono $$(topdir)/class/lib/$(PROFILE)/mcs.exe' > /dev/null || (echo FAILED; exit 1)

PROFILER=default

do-gettext:
	xgettext --keyword='Report.Error:3' --keyword='Report.Error:2' --keyword='Report.Warning:3' --keyword='Report.Warning:2' -o mcs.po --language='C#' `cat gmcs.exe.sources | grep -v /`

profile : $(PROGRAM)
	$(RUNTIME) $(RUNTIME_FLAGS) --profile=$(PROFILER) $(PROGRAM) $(USE_MCS_FLAGS) -target:exe -out:mcs2.exe $(BUILT_SOURCES) @$(response)

#
# quick hack target, to quickly develop the gmcs compiler
# Update manually.

q: cs-parser.cs qh
	echo 'System.Console.WriteLine ("Hello");' | mono csharp.exe
	echo -e 'using System;\nConsole.WriteLine ("hello");' | mono csharp.exe
	echo -e '"foo" == "bar";' | mono csharp.exe
	echo -e 'var a = 1;\na + 2;' | mono csharp.exe
	echo -e 'int j;\nj = 1;' | mono csharp.exe
	echo -e 'var a = new int[]{1,2,3};\nfrom x in a select x;' | mono csharp.exe
	echo -e 'var a = from f in System.IO.Directory.GetFiles ("/tmp") where f == "passwd" select f;' | mono csharp.exe


