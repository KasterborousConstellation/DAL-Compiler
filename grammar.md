# Grammaire du langage DAL 
$$
\begin{align*}
[\text{Prog}] &\to [\text{Ln}]^* \\
[\text{Ln}] &\to 
    \begin{cases}
        \text{FtcCall}\\
        \text{Afct}
    \end{cases}
    \\
[\text{FtcCall}] &\to \text{Idt}(\text{Args})\\
[\text{Args}] &\to \text{Expr},...,\text{Expr}
\\
[\text{Afct}] &\to \text{Idt} ~\text{<-}~ \text{Expr}\\
[\text{Expr}] &\to 
    \begin{cases}
        \text{Lit}\\
        \text{FtcCall}\\
        \text{Idt}\\
        \text{Oper}
    \end{cases}
\\
[\text{Oper}] &\to
    \begin{cases}
        \text{Optr} ~ \text{Expr}\\
        \text{Expr} ~ \text{Optr} ~ \text{Expr}
    \end{cases}
\end{align*}\\
$$
## Lexique
La table suivante définit les termes utilisés dans la grammaire.  
$$
\begin{align*}
    \text{Prog} &: \text{Programme}\\
    \text{Ln} &: \text{Ligne}\\
    \text{FtcCall} &: \text{Appel de fonction}\\
    \text{Args} &: \text{Arguments}\\
    \text{Afct} &: \text{Affectation}\\
    \text{Idt} &: \text{Identificateur}\\
    \text{Expr} &: \text{Expression}\\
    \text{Lit} &: \text{Litteral}\\
    \text{Oper} &: \text{Operation}\\
    \text{Optr} &: \text{Operateur}
\end{align*}\\
$$