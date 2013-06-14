NAntFind
========

    <find module="SQLServer" required="true" />
    <echo message="${SQLServer.found}" />

NAntFind is a c CMake find style dependency discovery extension for NAnt.

You can define your own find module like below:

    <module>
      <path>
        <pathelement dir="C:\Program Files\Microsoft SQL Server\100\Tools\Binn" />
        <pathelement dir="C:\Program Files (x86)\Microsoft SQL Server\100\Tools\Binn" />
      </path>
      <files>
        <file name="SQLCmd.exe" />
      </files>
    </module>

TODO
----
* Implementation
* Remove dependency to NAnt, but can be used in NAnt
* Search for find modules in ${find.module.path}
* Search for module of specific version
* A more flexible and easy to use DSL
* Support *which* for searching only one file, while *find* searches for package

License
----
[GNU Public License][1]


  [1]: http://www.gnu.org/copyleft/gpl.html
