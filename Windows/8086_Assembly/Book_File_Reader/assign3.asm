; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл
;           James Brooks CST250 Assignment 3 Book Sorting Program
;                 Summer 2012, OIT Instructor Pete Myers
;
;           <revisit>  find fixed font solution so columns line up.
;                      procedurize the code, use local variables.
;
;           notes      debugged in VS2010, but couldn't get resource
;                      compiler to work there.
;                      
;
; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

      .486                      ; create 32 bit code
      .model flat, stdcall      ; 32 bit memory model
      option casemap :none      ; case sensitive

; ддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддд

;     include files
;     ~~~~~~~~~~~~~
      include c:\masm32\include\windows.inc
      include c:\masm32\include\masm32.inc
      include c:\masm32\include\gdi32.inc
      include c:\masm32\include\user32.inc
      include c:\masm32\include\kernel32.inc
      include c:\masm32\include\Comctl32.inc
      include c:\masm32\include\comdlg32.inc
      include c:\masm32\include\shell32.inc
      include c:\masm32\include\oleaut32.inc
      include c:\masm32\include\msvcrt.inc
      include c:\masm32\macros\macros.asm

;     libraries
;     ~~~~~~~~~
      includelib c:\masm32\lib\masm32.lib
      includelib c:\masm32\lib\gdi32.lib
      includelib c:\masm32\lib\user32.lib
      includelib c:\masm32\lib\kernel32.lib
      includelib c:\masm32\lib\Comctl32.lib
      includelib c:\masm32\lib\comdlg32.lib
      includelib c:\masm32\lib\shell32.lib
      includelib c:\masm32\lib\oleaut32.lib
      includelib c:\masm32\lib\msvcrt.lib

; ддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддд

        ;=================
        ; Local prototypes
        ;=================
        WinMain          PROTO :DWORD,:DWORD,:DWORD,:DWORD
        WndProc          PROTO :DWORD,:DWORD,:DWORD,:DWORD
        TopXY            PROTO :DWORD,:DWORD
        Paint_Proc       PROTO :DWORD
        RegisterWinClass PROTO :DWORD,:DWORD,:DWORD,:DWORD,:DWORD
        MsgLoop          PROTO
        Main             PROTO

      AutoScale MACRO swidth, sheight
        invoke GetPercent,sWid,swidth
        mov Wwd, eax
        invoke GetPercent,sHgt,sheight
        mov Wht, eax

        invoke TopXY,Wwd,sWid
        mov Wtx, eax

        invoke TopXY,Wht,sHgt
        mov Wty, eax
      ENDM

      DisplayMenu MACRO handl, IDnum
        invoke LoadMenu,hInstance,IDnum
        invoke SetMenu,handl,eax
      ENDM

      DisplayWindow MACRO handl, ShowStyle
        invoke ShowWindow,handl, ShowStyle
        invoke UpdateWindow,handl
      ENDM

    .data
    
        szDisplayName db "Assignment 3 - Book File Reader - James Brooks CST250 OIT Summer 2012 Instructor Pete Myers",0

	  szHeader1 db "Title                           Author                          Published  ",13,10,0
	  szHeader2 db "-----                           ------                          ---------  ",13,10,0

	  booktitleaddresses dd 25 dup (0)
	  bookauthoraddresses dd 25 dup (0)
	  bookdateaddresses dd 25 dup (0)
	  bookdatenumericvalues dd 25 dup (0)

	  monthstrings db "JANFEBMARAPRMAYJUNJULAUGSEPOCTNOVDEC"

	  addressindexsort dd 25 dup(0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24)

	  tempstring db 5 dup (0)

 	  printbuffer db 2200 dup (" ")

    .data?

        hInstance dd ?
        CommandLine dd ?
        hIcon dd ?
        hCursor dd ?
        sWid dd ?
        sHgt dd ?
        hWnd dd ?

        infilenamepointer dd ?
        infilebufaddress dd ?
        infilelength dd ?
        bookcount dd ?
        booktitleaddressespointer dd ?
        bookauthoraddressespointer dd ?
        bookdateaddressespointer dd ?
	  currentfileposition dd ?
	  tempvalue dd ?
	  swap_flag dd ?
	  printbufferlocation dd ?


 
; ддддддддддддддддддддддддддд Inserted modules дддддддддддддддддддддддддддд

      include C:\masm32\tools\bintodb\Statusbr.asm

; ддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддддд



.code

start:

; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

      invoke InitCommonControls

    ; ------------------
    ; set global values
    ; ------------------
      mov hInstance,   rv(GetModuleHandle, NULL)
      mov CommandLine, rv(GetCommandLine)
      mov hIcon,       rv(LoadIcon,hInstance,500)
      mov hCursor,     rv(LoadCursor,NULL,IDC_ARROW)
      mov sWid,        rv(GetSystemMetrics,SM_CXSCREEN)
      mov sHgt,        rv(GetSystemMetrics,SM_CYSCREEN)

      call Main

      invoke ExitProcess,eax

; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

Main proc

    LOCAL Wwd:DWORD,Wht:DWORD,Wtx:DWORD,Wty:DWORD

    STRING szClassName,"ProStart_Class"

  ; --------------------------------------------
  ; register class name for CreateWindowEx call
  ; --------------------------------------------
    invoke RegisterWinClass,ADDR WndProc,ADDR szClassName,
                       hIcon,hCursor,COLOR_BTNFACE+1

  ; -------------------------------------------------
  ; macro to autoscale window co-ordinates to screen
  ; percentages and centre window at those sizes.
  ; -------------------------------------------------
    AutoScale 75, 70

    invoke CreateWindowEx,WS_EX_LEFT or WS_EX_ACCEPTFILES,
                          ADDR szClassName,
                          ADDR szDisplayName,
                          WS_OVERLAPPEDWINDOW,
                          Wtx,Wty,Wwd,Wht,
                          NULL,NULL,
                          hInstance,NULL
    mov hWnd,eax

  ; ---------------------------
  ; macros for unchanging code
  ; ---------------------------
    DisplayMenu hWnd,600
    DisplayWindow hWnd,SW_SHOWNORMAL
    call MsgLoop
    ret

Main endp

; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

RegisterWinClass proc lpWndProc:DWORD, lpClassName:DWORD,
                      Icon:DWORD, Cursor:DWORD, bColor:DWORD

    LOCAL wc:WNDCLASSEX

    mov wc.cbSize,         sizeof WNDCLASSEX
    mov wc.style,          CS_BYTEALIGNCLIENT or \
                           CS_BYTEALIGNWINDOW
    m2m wc.lpfnWndProc,    lpWndProc
    mov wc.cbClsExtra,     NULL
    mov wc.cbWndExtra,     NULL
    m2m wc.hInstance,      hInstance
    m2m wc.hbrBackground,  bColor
    mov wc.lpszMenuName,   NULL
    m2m wc.lpszClassName,  lpClassName
    m2m wc.hIcon,          Icon
    m2m wc.hCursor,        Cursor
    m2m wc.hIconSm,        Icon

    invoke RegisterClassEx, ADDR wc

    ret

RegisterWinClass endp

; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

MsgLoop proc

    LOCAL msg:MSG

    push esi
    push edi
    xor edi, edi                        ; clear EDI
    lea esi, msg                        ; Structure address in ESI
    jmp jumpin

    StartLoop:
      invoke TranslateMessage, esi
    ; --------------------------------------
    ; perform any required key processing here
    ; --------------------------------------
      invoke DispatchMessage,  esi
    jumpin:
      invoke GetMessage,esi,edi,edi,edi
      test eax, eax
      jnz StartLoop

    mov eax, msg.wParam
    pop edi
    pop esi

    ret

MsgLoop endp

; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

WndProc proc hWin:DWORD,uMsg:DWORD,wParam:DWORD,lParam:DWORD

    LOCAL var    :DWORD
    LOCAL caW    :DWORD
    LOCAL caH    :DWORD
    LOCAL fname  :DWORD
    LOCAL patn   :DWORD
    LOCAL Rct    :RECT
    LOCAL buffer1[260]:BYTE  ; these are two spare buffers
    LOCAL buffer2[260]:BYTE  ; for text manipulation etc..

    Switch uMsg
      Case WM_COMMAND

        Switch wParam
        ;======== menu commands ========

          Case 1001

		      ;get a book file name from the user

                  sas patn, "All files",0,"*.*",0
                  mov fname, OpenFileDlg(hWin,hInstance,"Open File",patn)
                  cmp BYTE PTR [eax], 0
                  jne @F
                  return 0
                  @@:
                
                  ; ---------------------------------
                  ; perform your file open code here
                  ; ---------------------------------

; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл


                  ;read in the book file contents

                  mov eax, fname
                  mov infilenamepointer, eax
                  invoke read_disk_file,infilenamepointer,ADDR infilebufaddress,ADDR infilelength
            

			;process the file

			mov esi, infilebufaddress 


			;convert the book count ascii to numeric  Note: bookcount <= 25

			mov bl, -1			                              ;set the hi ignore flag
			mov al, [esi]		                              ;get first byte of count
			inc esi
			sub al, "0"			                              ;convert to numeric
			mov bh, al
			mov al, [esi]		                              ;get next byte of count maybe
			inc esi
			cmp al, 13			                              ;check for eol
			je  bookcountloop1
			sub al, "0"			                              ;convert low byte to numeric
			mov bl, al

bookcountloop1:

			cmp bl, -1
			je  bookcountloop2	                              ;jump if no high byte
			sub eax,eax
			mov al,bh			
			mov bh, 10			                              ;mul hi byte by 10
			mul bh
			mov bh, al
			add bh, bl			                              ;add in low byte

bookcountloop2:

			sub eax, eax
			mov al, bh
			mov bookcount, eax

			mov al, [esi]		                              ;Adjust file pointer to point to first title
			cmp al, 13			
			jne bookcountloop3
			inc esi

bookcountloop3:

			inc esi
			mov currentfileposition, esi


; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл


			;Get addresses for all book file components            ;Note file position and eol assumptions are strong here

			;initialize address array indicies

			mov booktitleaddressespointer, 0
			mov bookauthoraddressespointer, 0
			mov bookdateaddressespointer, 0

			mov ecx, bookcount		;set up loop count
			mov esi, currentfileposition
			mov edi, booktitleaddressespointer

 makebookpointers1:

			mov [booktitleaddresses + EDI * 4], esi
			inc edi
			mov booktitleaddressespointer, edi
			mov edi, bookauthoraddressespointer

 makebookpointers2:

			mov al,[esi]
			inc esi
			cmp al, 10
			je  makebookpointers3
			jmp makebookpointers2
			
makebookpointers3:

			mov [bookauthoraddresses + EDI * 4], esi
			inc edi
			mov bookauthoraddressespointer, edi
			mov edi, bookdateaddressespointer

makebookpointers4:

			mov al,[esi]
			inc esi
			cmp al, 10
			je  makebookpointers5
			jmp makebookpointers4

makebookpointers5:

			mov [bookdateaddresses + EDI * 4], esi
			inc edi
			mov bookdateaddressespointer, edi
			mov edi, booktitleaddressespointer

makebookpointers6:

			mov al,[esi]
			inc esi
			cmp al, 10
			je  makebookpointers7
			jmp makebookpointers6

makebookpointers7:

			loop makebookpointers1			

			;rewind the file and indicies

			mov eax, infilebufaddress
			mov currentfileposition, eax

			mov booktitleaddressespointer, 0
			mov bookauthoraddressespointer, 0
			mov bookdateaddressespointer, 0


; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

			;Convert the date values to numeric

			;Work on year field

numericdate0:

			mov edi, bookdateaddressespointer
			mov esi, [bookdateaddresses + EDI * 4]
			add esi, 7								 ;put year field into zero terminated string

			mov ecx, 4
			lea edi, tempstring

numericdate1:

			mov eax, [esi]
			mov [edi], eax
			inc esi
			inc edi
			loop numericdate1
			sub eax, eax
			mov [edi], al							  ;zero terminate the string
			sub edi, 4								  ;rewind to beginning

			mov tempvalue, uval(edi)				        ;Year string to int
			mov eax, tempvalue
			mov bx, 10000
			mul bx
			shl edx,16
			mov dx,ax
			mov tempvalue,edx						        ;Year to numeric * 10,000


			;Work on Month field


numericdate2:

			mov edi, bookdateaddressespointer
			mov esi, [bookdateaddresses + EDI * 4]

			mov ecx, 3
			lea edi, tempstring


numericdate3:

			mov eax, [esi]							   ;point to month field
			mov [edi], eax
			inc esi
			inc edi
			loop numericdate3
			sub eax, eax
			mov [edi], al							   ;zero terminate the string
			sub edi, 3								   ;rewind to beginning
			
			mov edi, ucase$(edi)					         ;Take it to upper case

			mov ebx, 1								   ;Number date repre in ebx
			mov edx, 0								   ;Found match flag in edx
			lea esi, monthstrings					         ;Match against these strings

numericdate4:

			mov ecx, 3								   ;3 char match

numericdate5:

			mov al,[edi]
			cmp al,[esi]
			je  numericdate6
			mov edx, 1								   ;flag no match here

numericdate6:

			inc edi
			inc esi
			loop numericdate5

			cmp edx, 0								    ;got a match?
			je numericdate7
			cmp ebx, 12								    ;bail if no match by december
			je numericdate7
			inc ebx								    ;number rep for next month
			mov edx, 0
			sub edi, 3
			jmp numericdate4

numericdate7:
				
			mov eax, ebx							    ;add in month value to year
			mov bl, 100
			mul bl
			add eax, tempvalue
			mov tempvalue, eax


numericdate8:

			;Work on day field

			mov edi, bookdateaddressespointer
			mov esi, [bookdateaddresses + EDI * 4]
			add esi, 4								    ;put day field into zero terminated string

			mov ecx, 2
			lea edi, tempstring

numericdate9:

			mov eax, [esi]
			mov [edi], eax
			inc esi
			inc edi
			loop numericdate9
			sub eax, eax
			mov [edi], al							     ;zero terminate the string
			sub edi, 2								     ;rewind to beginning

			mov eax, uval(edi)						     ;Day string to int
			add eax, tempvalue
			mov tempvalue, eax

			mov edi, bookdateaddressespointer
			mov [bookdatenumericvalues  + EDI * 4], eax		     ;save date numeric in its array

			;Point to next book and do it all again

			mov eax, bookdateaddressespointer		                 ;set pointer to next book
			inc eax
			mov bookdateaddressespointer, eax
			cmp eax, bookcount						     ;did we run out of books?
			je numericdate10
			jmp numericdate0						           ;go do another

numericdate10:								

			sub eax,eax
			mov bookdateaddressespointer, eax		                 ;rewind the indexer
			lea eax, bookdatenumericvalues			           ;for debugging


; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

			;bubble sort the dates and the surrogate addressindexsort
			
			mov swap_flag, TRUE						      ;Initialize swap flag as true to kickstart loop

bubble1:

			mov EAX, swap_flag						      ;See if we did a swap last loop as test for done
			sub EAX, FALSE
			jz  bubble4

			mov swap_flag,FALSE						      ;set swap flag to false
			mov ECX, bookcount						      ;Set up loop count for bookcount - 1 compares
			dec ECX
			mov EDI, 0								      ;Initialize array indexing

bubble2:

			;compare array[n] to array[n+1]

			lea EDX, [bookdatenumericvalues + EDI * 4]                  ;load address of array + current offset for array[n]
			mov EBX, [EDX+4]							      ;get array[n+1]
       
			sub EBX, [EDX]								;Test for array[n+1] >= array[n]
			jge bubble3									;jump if array[n+1] >= array[n]

			;Do the bubble swap and set the swap flag 

			mov EAX, [EDX]
			mov EBX, [EDX+4] 
			mov [EDX], EBX
			mov [EDX+4], EAX

			;swap the surrogate too

			lea EDX, [addressindexsort + EDI * 4]                       ;load address of array + current offset for array[n]

			mov EAX, [EDX]
			mov EBX, [EDX+4] 
			mov [EDX], EBX
			mov [EDX+4], EAX

			mov swap_flag, TRUE

bubble3:

			inc EDI									;bump array index
			loop bubble2							      ;run through n-1 entries
			jmp bubble1								      ;repeat until no more swaps
     
bubble4:          lea  eax, addressindexsort				            ;for debugging
 

; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

			;Set up print buffer

			
			mov esi, 0

			;mov printbufferlocation, append$(printbuffer,szHeader1,printbufferlocation)	;put the header lines in
			;mov printbufferlocation, append$(printbuffer,szHeader2,printbufferlocation)	
			;mov printbuffer, cat$(printbuffer,szHeader1,szHeader2)
			;mov printbufferlocation, add$(printbuffer,szHeader1)

			;none of the above work so doing it the old passioned way

printbufloop1:

			mov esi, 0
			mov edi, 0
			mov printbufferlocation, edi

printbufloop2:

			mov al, [szHeader1 + esi]  
			cmp al, 0
			je printbufloop3
			mov [printbuffer + edi], al
			inc esi
			inc edi
			jmp printbufloop2

printbufloop3:

			lea eax, printbuffer							     ;for debugging
			mov printbufferlocation, edi
			mov esi, 0

printbufloop4:

			mov al, [szHeader2 + esi]  
			cmp al, 0
			je printbufloop5
			mov [printbuffer + edi], al
			inc esi
			inc edi
			jmp printbufloop4

printbufloop5:

			lea eax, printbuffer							     ;for debugging
			mov printbufferlocation, edi
			mov esi, 0

			
			mov ecx, bookcount

			sub eax, eax
			mov tempvalue, eax							     ;set up pointer into surrogate


printbufloop6:

			mov esi, tempvalue

			mov edx, [addressindexsort + esi * 4]			           ;get address index for next book to be printed

			mov booktitleaddressespointer, edx				           ;load up the pointers
			mov bookauthoraddressespointer, edx
			mov bookdateaddressespointer, edx

			;copy the title of the current book into print buffer

			;calc the print buffer position

			mov edi, 154								     ;after header offset
			mov eax, tempvalue							     ;get current line - assumes count <= 25
			mov bl, 79									     ;chars per line
			mul bl
			add edi, eax									

			mov esi, booktitleaddressespointer
			lea edx, [booktitleaddresses + esi * 4]
			mov edx, [edx]

printbufloop7:			

			mov al, [edx]
			cmp al, 13
			je	printbufloop8	
			mov [printbuffer + edi], al
			inc edi
			inc edx
			jmp printbufloop7

printbufloop8:			

			;copy the author of the current book into print buffer

			mov esi, bookauthoraddressespointer
			lea edx, [bookauthoraddresses + esi * 4]
			mov edx, [edx]

			;calc the print buffer position

			mov edi, 187									;after header offset
			mov eax, tempvalue								;get current line - assumes count <= 25
			mov bl, 79										;chars per line
			mul bl
			add edi, eax									

			mov esi, bookauthoraddressespointer
			lea edx, [bookauthoraddresses + esi * 4]
			mov edx, [edx]

printbufloop9:			

			mov al, [edx]
			cmp al, 13
			je	printbufloop10	
			mov [printbuffer + edi], al
			inc edi
			inc edx
			jmp printbufloop9

printbufloop10:			

			;copy the date of the current book into print buffer

			mov esi, bookdateaddressespointer
			lea edx, [bookdateaddresses + esi * 4]
			mov edx, [edx]

			;calc the print buffer position

			mov edi, 220							            ;after header offset
			mov eax, tempvalue								;get current line - assumes count <= 25
			mov bl, 79										;chars per line
			mul bl
			add edi, eax									

			mov esi, bookdateaddressespointer
			lea edx, [bookdateaddresses + esi * 4]
			mov edx, [edx]

printbufloop11:			

			mov al, [edx]
			cmp al, 13
			je	printbufloop12	
			mov [printbuffer + edi], al
			inc edi
			inc edx
			jmp printbufloop11

printbufloop12:			

			;Put in new line after date

			inc edi
			mov [printbuffer + edi], 13
			inc edi
			mov [printbuffer + edi], 10

			;Add a zero terminator if this is the last book

			cmp cx, 1
			jne printbufloop13
			inc edi
			mov [printbuffer + edi], 0

printbufloop13:

			;Setup for next book

			mov eax, tempvalue
			inc eax
			mov tempvalue, eax

			;loop printbufloop6							;got an assembly error of jump dest too far!

			dec cx									;loop workaround
			cmp cx, 0
			je printbufloop14
			jmp printbufloop6

printbufloop14:
			lea edx, printbuffer						       ;for debugging


                  ;Reset the surrogate array for next file

                  mov ecx, 25
                  mov eax, 0
                  mov edi, 0

printbufloop15:

                  mov [addressindexsort + edi * 4],eax
                  inc eax
                  inc edi
                  loop printbufloop15
                  
; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

		     ;Show the sorted results

                 invoke MessageBox,hWin,ADDR printbuffer,ADDR szDisplayName,MB_OK


                 ;clear the print buffer for next file
                 
                  mov ecx, 2200
                  mov eax, 32
                  mov edi, 0

printbufloop16:

                  mov [printbuffer + edi],al
                  inc edi
                  loop printbufloop16
   

                 ;End file open processing
; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл


          Case 1010
                  invoke SendMessage,hWin,WM_SYSCOMMAND,SC_CLOSE,NULL

          Case 1900
                  invoke MessageBox,hWin,ADDR szDisplayName,ADDR szDisplayName,MB_OK

          ;====== end menu commands ======
      Endsw

      Case WM_DROPFILES
                  mov fname, DropFileName(wParam)
                  fn MessageBox,hWin,fname,"WM_DROPFILES",MB_OK

      Case WM_CREATE
                  invoke Do_Status,hWin

      Case WM_SYSCOLORCHANGE

      Case WM_SIZE
                  invoke MoveWindow,hStatus,0,0,0,0,TRUE

      Case WM_PAINT
                  invoke Paint_Proc,hWin
                  return 0

      Case WM_CLOSE

      Case WM_DESTROY
                  invoke PostQuitMessage,NULL
                  return 0

    Endsw

    invoke DefWindowProc,hWin,uMsg,wParam,lParam

    ret

WndProc endp

; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

OPTION PROLOGUE:NONE 
OPTION EPILOGUE:NONE 

TopXY proc wDim:DWORD, sDim:DWORD

    mov eax, [esp+8]
    sub eax, [esp+4]
    shr eax, 1

    ret 8

TopXY endp

OPTION PROLOGUE:PrologueDef 
OPTION EPILOGUE:EpilogueDef 

; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

Paint_Proc proc hWin:DWORD

    LOCAL hDC      :DWORD
    LOCAL btn_hi   :DWORD
    LOCAL btn_lo   :DWORD
    LOCAL Rct      :RECT
    LOCAL Ps       :PAINTSTRUCT

    mov hDC, rv(BeginPaint,hWin,ADDR Ps)

  ; ----------------------------------------

    mov btn_hi, rv(GetSysColor,COLOR_BTNHIGHLIGHT)

    mov btn_lo, rv(GetSysColor,COLOR_BTNSHADOW)

  ; ----------------------------------------

    invoke EndPaint,hWin,ADDR Ps

    ret

Paint_Proc endp

; ллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллллл

end start
