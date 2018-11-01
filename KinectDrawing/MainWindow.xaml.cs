using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Media;

namespace ThaiXylophone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        private KinectSensor _sensor = null;
        private ColorFrameReader _colorReader = null;
        private BodyFrameReader _bodyReader = null;
        private IList<Body> _bodies = null;
    

        private int _width = 0;
        private int _height = 0;
        private byte[] _pixels = null;
        private WriteableBitmap _bitmap = null;


        public MainWindow()
        {
            InitializeComponent();

            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _width = _sensor.ColorFrameSource.FrameDescription.Width;
                _height = _sensor.ColorFrameSource.FrameDescription.Height;

                _colorReader = _sensor.ColorFrameSource.OpenReader();
                _colorReader.FrameArrived += ColorReader_FrameArrived;

                _bodyReader = _sensor.BodyFrameSource.OpenReader();
                _bodyReader.FrameArrived += BodyReader_FrameArrived;

                _pixels = new byte[_width * _height * 4];
                _bitmap = new WriteableBitmap(_width, _height, 96.0, 96.0, PixelFormats.Bgra32, null);

                _bodies = new Body[_sensor.BodyFrameSource.BodyCount];

                camera.Source = _bitmap;
            }
        }
        private void DrawingRanadTeeth(Rectangle recName, double width, double height, double positionOnXAxis, double positionOnYAxis)
        {
            recName.StrokeThickness = 2;
            recName.Stroke = Brushes.GreenYellow;
            recName.Width = width;
            recName.Height = height;
            Canvas.SetLeft(recName, positionOnXAxis);
            Canvas.SetTop(recName, positionOnYAxis);


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_colorReader != null)
            {
                _colorReader.Dispose();
            }

            if (_bodyReader != null)
            {
                _bodyReader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        private void ColorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    frame.CopyConvertedFrameDataToArray(_pixels, ColorImageFormat.Bgra);

                    _bitmap.Lock();
                    Marshal.Copy(_pixels, 0, _bitmap.BackBuffer, _pixels.Length);
                    _bitmap.AddDirtyRect(new Int32Rect(0, 0, _width, _height));
                    _bitmap.Unlock();
                }
            }
        }
       

        private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    frame.GetAndRefreshBodyData(_bodies);

                    Body body = _bodies.Where(b => b.IsTracked).FirstOrDefault();
                    frame.GetAndRefreshBodyData(_bodies);
            

                    if (body != null)
                    {
                        Joint handRight = body.Joints[JointType.HandRight];
                        Joint handLeft = body.Joints[JointType.HandLeft];

                        
                        

                        if (handRight.TrackingState != TrackingState.NotTracked && handLeft.TrackingState != TrackingState.NotTracked)
                        {
                            CameraSpacePoint handRightPosition = handRight.Position;
                            CameraSpacePoint handLeftPosition = handLeft.Position;
                           
                            ColorSpacePoint handRightPoint = _sensor.CoordinateMapper.MapCameraPointToColorSpace(handRightPosition);
                            ColorSpacePoint handLeftPoint = _sensor.CoordinateMapper.MapCameraPointToColorSpace(handLeftPosition);

                            float rightX = handRightPoint.X;
                            float rightY = handRightPoint.Y;
                            float leftX = handLeftPoint.X;
                            float leftY = handLeftPoint.Y;



                            //float leftX = handLeft.Position.X * (-100);
                            //float leftY = handLeft.Position.Y * 100;
                            //float leftZ = handLeft.Position.Z * 100;
                            // float leftX = handLeftPosition.X;
                            //float leftY = handLeftPosition.Y;

                            if (!float.IsInfinity(leftX) && !float.IsInfinity(leftY))
                            {
                                //trail.Points.Add(new Point { X = x, Y = y });
                                String rightHandState = "-";
                                String leftHandState = "-";
                                //left = System.Convert.ToString(y);
                                //right = System.Convert.ToString(x);



                                rightHandState = System.Convert.ToString("x: " + rightX + " , y: " + rightY);
                                leftHandState = System.Convert.ToString("x: " + leftX + " , y: " + leftY);

                                tblRightHandState.Text = rightHandState;
                                tblLeftHandState.Text = leftHandState;

                                //BrushConverter bc = new BrushConverter();
                                //Brush brush = (Brush)bc.ConvertFrom("Red");



                                Boolean alreadyTeeth1 = true;

                                if (((370<=leftX && leftX<=429)&&(690<=leftY && leftY<=930))||((370<=rightX && rightX <= 429)&&(933<=rightY && rightY <= 976 &&alreadyTeeth1))) 
                                {
                                    //Rectangle1.Fill = System.Windows.Media.Brushes.Red;
                                    Console.WriteLine("start :"+DateTime.Now.Second + "." + DateTime.Now.Millisecond);
                                    Rectangle[] allTeeth = { Rectangle2, Rectangle3, Rectangle4, Rectangle5, Rectangle6, Rectangle7, Rectangle8, Rectangle9, Rectangle10, Rectangle11, Rectangle12, Rectangle13, Rectangle14, Rectangle15, Rectangle16, Rectangle17, Rectangle18, Rectangle19, Rectangle20, Rectangle21, Rectangle22 };
                                    foreach(Rectangle teeth in allTeeth)
                                    {
                                        teeth.Fill = System.Windows.Media.Brushes.White;
                                    }
                                    Rectangle1.Fill = new SolidColorBrush(Colors.Red);

                                    //Rectangle1.Fill = new SolidColorBrush(Colors.Red);
                                    //int start = DateTime.Now.Millisecond;
                                    status.Text = "teeth1";
                                    SoundPlayer teeth1_sound = new SoundPlayer(@"C:\Users\Best-Dosl\Desktop\wav-piano-sound-master\wav\a1.wav");
                                    
                                    teeth1_sound.Play();



                                    Console.WriteLine("end :" + DateTime.Now.Second + "." + DateTime.Now.Millisecond);



                                    //System.Media.SoundPlayer teeth1_Sound = new System.Media.SoundPlayer(@"E:\soundForKinect\1.wav");
                                    //teeth1_Sound.Play();

                                }
                                else if (((434 <= leftX && leftX <= 470) && (704 <= leftY && leftY <= 975)) || ((434 <= rightX && rightX <= 470) && (704 <= rightY && rightY <= 975)))
                                {

                                    Rectangle[] allTeeth = { Rectangle1, Rectangle3, Rectangle4, Rectangle5, Rectangle6, Rectangle7, Rectangle8, Rectangle9, Rectangle10, Rectangle11, Rectangle12, Rectangle13, Rectangle14, Rectangle15, Rectangle16, Rectangle17, Rectangle18, Rectangle19, Rectangle20, Rectangle21, Rectangle22 };
                                    foreach(Rectangle teeth in allTeeth)
                                    {
                                        teeth.Fill = System.Windows.Media.Brushes.White;
                                    }

                                    Rectangle2.Fill = new SolidColorBrush(Colors.Red);
                                    status.Text = "teeth2";
                                    //System.Media.SoundPlayer teeth2_sound = new System.Media.SoundPlayer(@"C:\Users\Best-Dosl\Desktop\wav-piano-sound-master\wav\b1.wav");
                                    //teeth2_sound.Play();
                                   
                                }
                                else if (((480 <= leftX && leftX <= 520) && (740 <= leftY && leftY <= 920)) || ((480 <= rightX && rightX <= 520) && (740 <= rightY && rightY <= 920)))
                                {
                                    Rectangle[] allTeeth = { Rectangle1, Rectangle2, Rectangle4, Rectangle5, Rectangle6, Rectangle7, Rectangle8, Rectangle9, Rectangle10, Rectangle11, Rectangle12, Rectangle13, Rectangle14, Rectangle15, Rectangle16, Rectangle17, Rectangle18, Rectangle19, Rectangle20, Rectangle21, Rectangle22 };
                                    foreach (Rectangle teeth in allTeeth)
                                    {
                                        teeth.Fill = System.Windows.Media.Brushes.White;
                                    }
                                    Rectangle3.Fill = new SolidColorBrush(Colors.Red);
                                    status.Text = "teeth3";
                                    //System.Media.SoundPlayer teeth3_Sound = new System.Media.SoundPlayer(@"E:\soundForKinect\3.wav");
                                    //teeth3_Sound.Play();
                                }
                                else if (((530 <= leftX && leftX <= 560) && (745 <= leftY && leftY <= 1030)) || ((530 <= rightX && rightX <= 560) && (745 <= rightY && rightY <= 1030)))
                                {
                                    Rectangle[] allTeeth = { Rectangle1, Rectangle2, Rectangle3, Rectangle5, Rectangle6, Rectangle7, Rectangle8, Rectangle9, Rectangle10, Rectangle11, Rectangle12, Rectangle13, Rectangle14, Rectangle15, Rectangle16, Rectangle17, Rectangle18, Rectangle19, Rectangle20, Rectangle21, Rectangle22 };
                                    foreach (Rectangle teeth in allTeeth)
                                    {
                                        teeth.Fill = System.Windows.Media.Brushes.White;
                                    }
                                    Rectangle4.Fill = new SolidColorBrush(Colors.Red);
                                    status.Text = "teeth4";
                                    //System.Media.SoundPlayer teeth4_Sound = new System.Media.SoundPlayer(@"E:\soundForKinect\4.wav");
                                    //teeth4_Sound.Play();
                                }
                                
                                else
                                    status.Text = "wer";
                                    alreadyTeeth1 = false;

                            }

                                Canvas.SetLeft(stick, leftX - stick.Width/4);
                                Canvas.SetTop(stick, leftY - stick.Height*1.5);
                            
                        }
                                //
                                //if(leftX>-20 && leftY > -20)
                                //{
                                //canvas.Children.Remove(trail);

                                //  canvas.Children.Add(rectangle2);
                                //     canvas.Children.Add(rectangle3);
                                //     canvas.Children.Add(rectangle4);
                                //     canvas.Children.Add(rectangle5);
                                //     canvas.Children.Add(rectangle6);
                                //     canvas.Children.Add(rectangle7);
                                //     canvas.Children.Add(rectangle8);
                                //     canvas.Children.Add(rectangle9);
                                //     canvas.Children.Add(rectangle10);
                                //canvas.Children.Add(brush);


                            
                        }
                        
                    }
                }
            }
        }
        
    }

