/**
 * Based on Arcade-plus
 *
 * @source https://github.com/yojohanshinwataikei/Arcade-plus/blob/master/Assets/Scripts/Aff/ArcaeaFileFormat.g4
 * @author yojohanshinwataikei
 */

// $antlr-format alignTrailingComments true, columnLimit 150, minEmptyLines 1, maxEmptyLinesToKeep 1, reflowComments false, useTab false
// $antlr-format allowShortRulesOnASingleLine true, allowShortBlocksOnASingleLine true, alignSemicolons hanging, alignColons hanging

grammar ArcaeaAffChart;

// $antlr-format alignTrailingComments true, columnLimit 150, minEmptyLines 0, maxEmptyLinesToKeep 1, reflowComments false, useTab false
// $antlr-format allowShortRulesOnASingleLine true, allowShortBlocksOnASingleLine true, minEmptyLines 0, alignSemicolons ownLine
// $antlr-format alignColons trailing, singleLineOverrulesHangingColon true, alignLexerCommands true, alignLabels true, alignTrailers true

Whitespace: [\p{White_Space}] -> skip;

// LineComment 
//     : '//' ~[\r\n]*  ('\r'? '\n' | EOF)
//     -> skip
//     ;
// 
// BlockComment 
//     : '/*' .*? '*/'
//     -> skip
//     ;

LParen    : '(';
RParen    : ')';
LBrack    : '[';
RBrack    : ']';
LBrace    : '{';
RBrace    : '}';
LChev     : '<';
RChev     : '>';
Comma     : ',';
Semicolon : ';';
Colon     : ':';
Equal     : '=';
Underline : '_';

Space    : ' ';
Plus     : '+';
Minus    : '-';
Multiply : '*';
Divide   : '/';
Mod      : '%';
Pow      : '^';

fragment SQUOTE     : '\'';
fragment DQUOTE     : '"';
fragment BQUOTE     : '`';
fragment UNDERLINE  : '_';
fragment SHARP      : '#';
fragment ALPHABET   : [a-zA-Z];
fragment DIGITSTART : [1-9];
fragment ZERO       : '0';
fragment DIGIT      : DIGITSTART | ZERO;
fragment DOT        : '.';
fragment NEGATIVE   : '-';
fragment SPACE      : ' ';
fragment SLASH      : '/';
fragment BSLASH     : '\\';

// $antlr-format alignTrailingComments true, columnLimit 150, minEmptyLines 1, maxEmptyLinesToKeep 1, reflowComments false, useTab false
// $antlr-format allowShortRulesOnASingleLine false, allowShortBlocksOnASingleLine true, alignSemicolons hanging, alignColons hanging

chart
    : header+ '-' body EOF
    | body EOF
    ;

eventTiming : 'timing' LParen (Int Comma) (Float Comma) (Float) RParen;
eventTap : LParen (Int Comma) (Int | Float) RParen;
eventHold : 'hold' LParen (Int Comma) (Int Comma) (Int | Float) RParen;
eventArc : 'arc' LParen (Int Comma) (Int Comma) (Float Comma) (Float Comma) (Word Comma) (Float Comma) (Float Comma) (Int Comma) (Word Comma) (Word) (Comma Float)? RParen subEvents?;
eventArcTap : 'arctap' LParen (Int) RParen;
eventCamera : 'camera' LParen (Int Comma) (Float Comma) (Float Comma) (Float Comma) (Float Comma) (Float Comma) (Float Comma) (Word Comma) (Int) RParen;
eventScenecontrol : 'scenecontrol' LParen (Int Comma) (Word Comma) ((Float Comma) Int)? RParen;
eventTimingGroup : 'timinggroup' LParen (Word)? RParen segment;

event
    : eventTiming 
    | eventTap
    | eventHold
    | eventArc
    | eventArcTap
    | eventCamera
    | eventScenecontrol
    | eventTimingGroup
    ;

item
    : event Semicolon
    ;

subEvents
    : LBrack (event (Comma event)*)? RBrack
    ;

segment
    : LBrace body RBrace
    ;

header
    : Word Colon (Int | Float)
    ;

body
    : item*
    ;

fragment WORD_HEAD // Word must starts with '#', '_' or an Alpha
    : (SHARP | UNDERLINE | ALPHABET)
    ;

fragment WORD_BODY
    : (
        SHARP
        | UNDERLINE
        | ALPHABET
        | DIGIT
        | DOT
        | SLASH
        | BSLASH
        | NEGATIVE
        | SPACE // space can only be in the middle part of the Word
    )
    ;

fragment WORD_TAIL // space is not allowed as ending
    : (SHARP | UNDERLINE | ALPHABET | DIGIT | DOT | SLASH | BSLASH | NEGATIVE)
    ;

Word
    : WORD_HEAD (WORD_BODY* WORD_TAIL)?
    ;

Int
    : NEGATIVE? (ZERO | DIGITSTART DIGIT*)
    ;

Float
    : Int DOT DIGIT+
    ;
