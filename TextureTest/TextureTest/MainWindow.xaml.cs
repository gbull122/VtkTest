using Kitware.VTK;
using System.Windows;

namespace TextureTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vtkPoints points =vtkPoints.New();

            int GridSize = 10;
            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    points.InsertNextPoint(x, y, (x + y) / (y + 1));
                }
            }

            // Add the grid points to a polydata object
            vtkPolyData polydata =vtkPolyData.New();
            polydata.SetPoints(points);

            // Triangulate the grid points
            vtkDelaunay2D delaunay =vtkDelaunay2D.New();
            delaunay.SetInput(polydata);
            delaunay.Update();
            
            vtkPlaneSource plane = vtkPlaneSource.New();
            
            //Read the image data from a file
            vtkJPEGReader reader = vtkJPEGReader.New();
            reader.SetFileName("C:\\Users\\georg\\Desktop\\texture.jpg");

            //Create texture object
            vtkTexture texture = vtkTexture.New();
            texture.SetInputConnection(reader.GetOutputPort());

            //Map texture coordinates           
            vtkTextureMapToPlane map_to_plane = vtkTextureMapToPlane.New();
            map_to_plane.SetInputConnection(plane.GetOutputPort());
            map_to_plane.SetInputConnection(delaunay.GetOutputPort());
            
            //Create mapper and set the mapped texture as input
            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInputConnection(map_to_plane.GetOutputPort());

            //Create actor and set the mapper and the texture
            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.SetTexture(texture);

            vtkRenderer renderer = vtkRenderer.New();
            renderer.AddActor(actor);


            RenderControl1.RenderWindow.AddRenderer(renderer);
        }
    }
}
